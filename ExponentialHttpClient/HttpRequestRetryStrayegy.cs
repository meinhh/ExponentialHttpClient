using System;
using System.Net;

namespace ExponentialHttpClient
{
    internal class HttpRequestRetryStrayegy : IHttpRequestRetryStrayegy
    {
        public TimeSpan GetTimeToWaitBeforeNextRetry(int timesRetried)
        {
            throw new NotImplementedException();
        }

        public bool ShouldRetryAgain(int timesRetried)
        {
            throw new NotImplementedException();
        }

        public bool ShouldRetryOnException(Exception ex)
        {
            throw new NotImplementedException();
        }

        public bool ShouldRetryOnStatusCode(HttpStatusCode statusCode)
        {
            throw new NotImplementedException();
        }
    }
}