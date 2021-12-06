using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Library.Net;
using Serilog;
using Social.Domain.Twitter;

namespace Social.Infrastructure.Twitter
{
    internal class TwitterService : ITwitterService
    {
        private readonly HttpClient _client;
        private readonly TwitterConfiguration _configuration;
        private readonly ILogger _logger;

        public TwitterService(HttpClient client, TwitterConfiguration configuration, ILogger logger)
        {
            _client = client;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<TwitterUser?> GetUserByUsernameAsync(string username, CancellationToken token = default)
        {
            // Create token that will be cancellable either by the configured timeout or invoking code
            var timeout = new CancellationTokenSource(_configuration.RequestTimeout).Token;
            var requestToken = CancellationTokenSource.CreateLinkedTokenSource(timeout, token).Token;

            var url = $"{_configuration.BaseUrl}/users/by/username/{username}?user.fields=created_at,description,id,location,name,pinned_tweet_id,profile_image_url,protected,public_metrics,url,username,verified";
            var request = new HttpRequestMessage(HttpMethod.Get, url)
            {
                Headers =
                {
                    Authorization = new AuthenticationHeaderValue("Bearer", _configuration.BearerToken)
                }
            };

            // Get response string whether it is the expected or error response
            var response = await _client.SendAsync(request, HttpCompletionOption.ResponseContentRead, requestToken).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync(token).ConfigureAwait(false);

            if (response.StatusCode.IsSuccess())
            {
                TwitterV2Response<UserByUsernameData>? twitterResponse;
                try
                {
                    twitterResponse = JsonSerializer.Deserialize<TwitterV2Response<UserByUsernameData>>(content)!;
                    if (twitterResponse.Data != null)
                    {
                        var user = new TwitterUser
                        {
                            TwitterId = twitterResponse.Data.Id,
                            Name = twitterResponse.Data.Name,
                            Username = twitterResponse.Data.Username,
                            //Created = twitterResponse.Data.Created,
                            Description = twitterResponse.Data.Description,
                            //ProfileUrl = twitterResponse.Data.Url,
                            //ProfileImageUrl = twitterResponse.Data.ProfileImageUrl
                        };

                        return user;
                    }
                }
                catch (Exception e)
                {
                    throw new ApplicationException("An unexpected error occurred while deserializing a response from the Twitter V2 API.", e);
                }

                if (twitterResponse.Errors is { Count: > 0 })
                {
                    _logger.Error("");      // TODO: Log error data
                }
                return null;
            }

            ErrorResponse? error;
            try
            {
                error = JsonSerializer.Deserialize<ErrorResponse>(content);
            }
            catch (Exception e)
            {
                throw new HttpRequestException(String.Concat("Response status code does not indicate success: ", new HttpStatusCodeFormatter().Format("G", response.StatusCode, default)), e);
            }

            if (error != null) throw new TwitterErrorException(error!, response.StatusCode);
            
            throw new HttpRequestException(String.Concat("Response status code does not indicate success: ", new HttpStatusCodeFormatter().Format("G", response.StatusCode, default)));
        }
    }
}
