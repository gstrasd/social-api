﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Library.Dataflow;
using Library.Platform.Queuing;
using Serilog;
using Social.Domain.Twitter;
using Social.Infrastructure;
using Social.Infrastructure.Domain;
using Social.Messages;

namespace Social.Workers.Consumers
{
    internal class DiscoverTwitterAccountMessageConsumer : MessageConsumer<DiscoverTwitterAccountMessage>
    {
        private readonly ITwitterService _twitterService;
        private readonly ISocialMediaRepository _socialMediaRepository;
        private readonly IQueueClient _queueClient;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger _logger;

        public DiscoverTwitterAccountMessageConsumer(ITwitterService twitterService, ISocialMediaRepository socialMediaRepository, IQueueClient queueClient, ISourceBlock<DiscoverTwitterAccountMessage> buffer, JsonSerializerOptions serializerOptions, ILogger logger, CancellationToken token = default) :
            base(buffer, token)
        {
            _twitterService = twitterService;
            _socialMediaRepository = socialMediaRepository;
            _queueClient = queueClient;
            _serializerOptions = serializerOptions;
            _logger = logger;
        }

        protected override async Task ConsumeMessageAsync(DiscoverTwitterAccountMessage message, CancellationToken token = default)
        {
            try
            {
                _logger.Debug($"Message {message.CorrelationId} has been received.\n{JsonSerializer.Serialize(message, _serializerOptions)}");

                // Validate message
                var results = new List<ValidationResult>();
                if (!Validator.TryValidateObject(message, new ValidationContext(message), results, true))
                {
                    var error = new MessageValidationException("A discover-twitter-account message failed validation.", message, results);
                    _logger.Warning(error, "A discover-twitter-account message was received that cannot be processed.");
                    return;
                }

                // Find previous search result
                var previousSearch = await _socialMediaRepository.FindPreviousSearchAsync(message.TwitterUsername, token);
                var searchResult = previousSearch?.Results.FirstOrDefault(r => r.SocialMediaType == SocialMediaType.Twitter);
                if (searchResult is {Success: false})
                {
                    _logger.Information($"Twitter user {message.TwitterUsername} was previously searched and did not locate an account.");
                    return;
                }

                if (searchResult is {Success: true})
                {
                    if (searchResult.ProviderId == message.ProviderId)
                    {
                        _logger.Information($"Twitter user {message.TwitterUsername} already exists and associated with provider {message.ProviderId}.");
                    }
                    else
                    {
                        _logger.Warning($"Twitter user {message.TwitterUsername} was previously search and found to be associated with provider {searchResult.ProviderId}. However, the message indicates that it should belong to provider {message.ProviderId}. Please verify proper messaging and ad parsing logic.");
                    }
                    return;
                }

                // Find twitter user
                var user = await _twitterService.GetUserByUsernameAsync(message.TwitterUsername, token);

                // Save search result
                var search = previousSearch;
                if (search != null)
                {
                    search.Results.Add(new() { ProviderId = message.ProviderId, SocialMediaType = SocialMediaType.Twitter, Success = user != null });
                }
                else
                {
                    search = new Search
                    {
                        Value = message.TwitterUsername,
                        Results = new List<SearchResult>
                        {
                            new() {ProviderId = message.ProviderId, SocialMediaType = SocialMediaType.Twitter, Success = user != null}
                        }
                    };
                }
                _socialMediaRepository.SaveSearchAsync(search, token);

                // If a twitter user was not found, then end message processing
                if (user == null)
                {
                    _logger.Information($"Twitter user {message.TwitterUsername} could not be found.");
                    return;
                }

                // Associate this twitter user with supplied provider

                // Acquire tweets for this user
                var reconcileMessage = new ReconcileTweetsMessage
                {
                    CorrelationId = message.CorrelationId,
                    ProviderId = message.ProviderId,
                    TwitterUserId = user.Id
                };
                _queueClient.WriteMessageAsync(reconcileMessage, token);
            }
            catch (Exception e)
            {
                _logger.Error(e, $"An error occurred while trying to process message \"{message.CorrelationId}\" of type \"{message.GetType()}\". The message will be forwarded to the dead letter queue.");
            }
        }
    }
}