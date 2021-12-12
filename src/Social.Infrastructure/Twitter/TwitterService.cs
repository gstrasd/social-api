using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Library.Net;
using Serilog;
using Social.Domain.Twitter;

namespace Social.Infrastructure.Twitter
{
    internal class TwitterService : ITwitterService
    {
        private static readonly Regex _usernameValidationExpression = new("^[A-Za-z0-9_]{1,15}$", RegexOptions.Compiled);
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
            if (!_usernameValidationExpression.IsMatch(username))
            {
                throw new ArgumentException($"Cannot get user from Twitter V2 API. Parameter value: \"{username}\" must match expression \"{_usernameValidationExpression}\"", nameof(username));
            }

            var url = $"{_configuration.BaseUrl}/users/by/username/{username}?user.fields=created_at,description,id,location,name,pinned_tweet_id,profile_image_url,protected,public_metrics,url,username,verified";
            var data = await GetDataAsync<UserByUsernameData>(url, token);

            if (data == null) return null;
            
            var user = new TwitterUser
            {
                Id = data.Id,
                Name = data.Name,
                Username = data.Username,
                Created = data.Created,
                Description = data.Description,
                ProfileUrl = data.Url,
                ProfileImageUrl = data.ProfileImageUrl
            };

            return user;
        }

        public async Task<IEnumerable<Tweet>?> GetTweetsByUserId(string id, DateTime? startDate = default, DateTime? endDate = default, int maxCount = 50, CancellationToken token = default)      // TODO: Make this IAsyncEnumerable
        {
            if (maxCount < 1) throw new ArgumentOutOfRangeException(nameof(maxCount), maxCount, "At least one tweet must be requested.");
            if (maxCount > 100) throw new ArgumentOutOfRangeException(nameof(maxCount), maxCount, "The maximum number of tweets that can be retrieved in a single request is 100.");

            var url = new StringBuilder($"{_configuration.BaseUrl}/users/{id}/tweets?");
            if (startDate != default) url.Append($"start_time={startDate:O}&");
            if (endDate != default) url.Append($"end_time={endDate:O}&");
            url.Append("tweet.fields=id,text&");
            url.Append($"max_results={maxCount}");

            var data = await GetDataAsync<List<TweetData>>(url.ToString(), token);
            var tweets =
                from d in data
                select new Tweet
                {
                    TweetId = d.Id,
                    Text = d.Text
                };

            return tweets;
        }

        private async Task<TData?> GetDataAsync<TData>(string url, CancellationToken token = default)
        {
            // Create token that will be cancellable either by the configured timeout or invoking code
            var timeout = new CancellationTokenSource(_configuration.RequestTimeout).Token;
            var requestToken = CancellationTokenSource.CreateLinkedTokenSource(timeout, token).Token;

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
                TwitterV2Response<TData>? twitterResponse;
                try
                {
                    twitterResponse = JsonSerializer.Deserialize<TwitterV2Response<TData>>(content)!;
                    if (twitterResponse.Data != null) return twitterResponse.Data;
                }
                catch (Exception e)
                {
                    throw new ApplicationException("An unexpected error occurred while deserializing a response from the Twitter V2 API.", e);
                }

                if (twitterResponse.Errors is { Count: > 0 })
                {
                    _logger.Error("");      // TODO: Log error data
                }
                return default;
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
