using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Library.Net;

namespace Social.Infrastructure.Twitter
{
    /// <summary>
    /// Represents the structure of all Twitter responses
    /// </summary>
    /// <typeparam name="T">The type of data requested</typeparam>
    internal class TwitterV2Response<T>
    {
        [JsonPropertyName("data")]
        public T? Data { get; set; }
        [JsonPropertyName("errors")]
        public List<Error>? Errors { get; set; }
    }

    /// <summary>
    /// The error detail included with 200 responses
    /// </summary>
    internal class Error
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = default!;
        [JsonPropertyName("detail")]
        public string Detail { get; set; } = default!;
        [JsonPropertyName("type")]
        public Uri Type { get; set; } = default!;
    }

    /// <summary>
    /// The response served for non-200 errors
    /// </summary>
    internal class ErrorResponse
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = default!;
        [JsonPropertyName("detail")]
        public string Detail { get; set; } = default!;
        [JsonPropertyName("type")]
        public Uri Type { get; set; } = default!;
        [JsonPropertyName("errors")]
        public List<ErrorMessage> Errors { get; set; } = default!;
    }

    /// <summary>
    /// The error message included with non-200 errors
    /// </summary>
    internal sealed class ErrorMessage
    {
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}
