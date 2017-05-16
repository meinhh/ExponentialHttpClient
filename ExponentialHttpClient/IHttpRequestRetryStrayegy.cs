using System;
using System.Net;

namespace ExponentialHttpClient
{
    internal interface IHttpRequestRetryStrayegy
    {
        bool ShouldRetryOnStatusCode(HttpStatusCode statusCode);
        bool ShouldRetryOnException(Exception ex);
        TimeSpan GetTimeToWaitBeforeNextRetry(int timesRetried);
        bool ShouldRetryAgain(int timesRetried);
    }
}