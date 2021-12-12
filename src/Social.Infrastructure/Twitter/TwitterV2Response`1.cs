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
        public T? Data { get; set; }
        public List<Error>? Errors { get; set; }
    }

    /// <summary>
    /// The error detail included with 200 responses
    /// </summary>
    internal class Error
    {
        public string Title { get; set; } = default!;
        public string Detail { get; set; } = default!;
        public Uri Type { get; set; } = default!;
    }

    /// <summary>
    /// The response served for non-200 errors
    /// </summary>
    internal class ErrorResponse
    {
        public string Title { get; set; } = default!;
        public string Detail { get; set; } = default!;
        public Uri Type { get; set; } = default!;
        public List<ErrorMessage> Errors { get; set; } = default!;
    }

    /// <summary>
    /// The error message included with non-200 errors
    /// </summary>
    internal sealed class ErrorMessage
    {
        public string? Message { get; set; }
    }
}
