using System;

namespace ExponentialHttpClient.Exceptions
{
    [Serializable]
    internal class HttpRequestFailedException : Exception
    {
        public HttpRequestFailedException()
        {
        }

        public HttpRequestFailedException(string message) : base(message)
        {
        }

        public HttpRequestFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}