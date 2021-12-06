using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Library;

namespace Social.Infrastructure.Twitter
{
    internal class TwitterErrorException : ApplicationException
    {
        public TwitterErrorException(ErrorResponse? error, HttpStatusCode statusCode) : base(error?.Detail)
        {
            ErrorType = error?.Type;
            Errors = error == null
                ? new List<string>()
                : error.Errors.Where(e => String.IsNullOrWhiteSpace(e.Message)).Select(e => e.Message!).ToList();
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
        public Uri? ErrorType { get; }
        public List<string> Errors { get; }
    }
}