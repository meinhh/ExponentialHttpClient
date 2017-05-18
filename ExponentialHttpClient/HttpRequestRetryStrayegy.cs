using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExponentialHttpClient
{
    internal class HttpRequestRetryStrayegy : IHttpRequestRetryStrayegy
    {
        private const int DEFAULT_MAX_RETRIES = 15;
        private readonly HttpStatusCode[] _statusCodeToRetry =
        {
            HttpStatusCode.NotFound,
            HttpStatusCode.ServiceUnavailable,
            HttpStatusCode.RequestTimeout,
            HttpStatusCode.GatewayTimeout
        };
        private readonly int _maxRetries;

        public HttpRequestRetryStrayegy(int maxRetries = DEFAULT_MAX_RETRIES)
        {
            _maxRetries = maxRetries;
        }

        public TimeSpan GetTimeToWaitBeforeNextRetry(int timesRetried)
        {
            if (timesRetried < 0)
                return TimeSpan.Zero;

            var secondsToWait = Math.Pow(2, timesRetried);
            return TimeSpan.FromSeconds(secondsToWait);
        }

        public bool ShouldRetryAgain(int timesRetried)
        {
            return timesRetried < _maxRetries;
        }

        public bool ShouldRetryOnException(Exception ex)
        {
            if (ex is AggregateException)
            {
                var aggEx = ex as AggregateException;

                if (aggEx.InnerExceptions.Any(e => e.GetType() == typeof(TaskCanceledException)))
                    return true;
            }

            return ex is HttpRequestException || ex is TaskCanceledException;
        }

        public bool ShouldRetryOnStatusCode(HttpStatusCode statusCode)
        {
            return _statusCodeToRetry.Contains(statusCode);
        }
    }
}