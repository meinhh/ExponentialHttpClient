using System;

namespace ExponentialHttpClient.Exceptions
{
    [Serializable]
    public class HttpRequestFailedException : Exception
    {
        public HttpRequestFailedException(string url) : base($"Http request failed. url: {url}")
        {
        }

        public HttpRequestFailedException(string url, Exception ex) : base($"Http request failed. url: {url}", ex)
        {
        }
    }
}