using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ExponentialHttpClient.Exceptions;

namespace ExponentialHttpClient
{
    public class RetryingHttpClient : IRetryingHttpClient
    {
        private readonly HttpClient _client;
        private readonly ISleepService _sleepService;
        private readonly IHttpRequestRetryStrayegy _retryStrategy;

        public RetryingHttpClient()
            : this(new HttpClient(), new SleepService(), new HttpRequestRetryStrayegy())
        { }

        internal RetryingHttpClient(HttpClient client, ISleepService sleepService, IHttpRequestRetryStrayegy retryStrategy)
        {
            _client = client;
            _sleepService = sleepService;
            _retryStrategy = retryStrategy;
        }

        public async Task<string> PostAsJson<TSourceType>(string url, TSourceType data)
        {
            return await PostAsJson<TSourceType, string>(url, data);
        }

        public async Task<TDestinationType> PostAsJson<TSourceType, TDestinationType>(string url, TSourceType data)
        {
            var dataSerialized = JsonConvert.SerializeObject(data);
            var content = new StringContent(dataSerialized, Encoding.UTF8, "application/json");
            return await GetData<TDestinationType>(url, async () => await _client.PostAsync(url, content));
        }

        public async Task<TDestinationType> Get<TDestinationType>(string url)
        {
            return await GetData<TDestinationType>(url, async () => await _client.GetAsync(url));
        }

        private async Task<TDestinationType> GetData<TDestinationType>(string url, Func<Task<HttpResponseMessage>> getResponse)
        {
            ThrowIfNull(url);

            try
            {
                return await TryRequest<TDestinationType>(url, getResponse);
            }
            catch (ClientShouldRetryException) { }

            var timesRetried = 0;

            while (_retryStrategy.ShouldRetryAgain(timesRetried))
            {
                await SleepBeforeNextRetry(timesRetried, url);

                try
                {
                    return await TryRequest<TDestinationType>(url, getResponse);
                }
                catch (ClientShouldRetryException) { }

                timesRetried++;
            }

            throw new HttpRequestFailedException(url, new Exception($"Http request failed { timesRetried + 1 } times."));
        }

        private async Task SleepBeforeNextRetry(int timesRetried, string url)
        {
            var timeToSleep = _retryStrategy.GetTimeToWaitBeforeNextRetry(timesRetried);
            await _sleepService.Sleep(Convert.ToInt32(Math.Floor(timeToSleep.TotalMilliseconds)));
        }

        private async Task<TDestinationType> TryRequest<TDestinationType>(string url, Func<Task<HttpResponseMessage>> getResponse)
        {
            try
            {
                var res = await getResponse();
                return await GetResponseContent<TDestinationType>(res, url);
            }
            catch (ClientShouldRetryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (_retryStrategy.ShouldRetryOnException(ex))
                    throw new ClientShouldRetryException(url, ex);

                throw new HttpRequestFailedException(url, ex);
            }
        }

        private async Task<TDestinationType> GetResponseContent<TDestinationType>(HttpResponseMessage res, string url)
        {
            if (res.IsSuccessStatusCode)
                return await ReadResponseContent<TDestinationType>(res.Content);

            if (_retryStrategy.ShouldRetryOnStatusCode(res.StatusCode))
                throw new ClientShouldRetryException(url);

            throw new Exception($"Http request failed. Url: {url}. StatusCode: {res.StatusCode}. Error: {res.ReasonPhrase}.");
        }

        private async Task<TDestinationType> ReadResponseContent<TDestinationType>(HttpContent content)
        {
            var contentAsStr = await content.ReadAsStringAsync();

            if (typeof(TDestinationType) == typeof(string))
                return (TDestinationType)Convert.ChangeType(contentAsStr, typeof(TDestinationType));

            return JsonConvert.DeserializeObject<TDestinationType>(contentAsStr);
        }

        private void ThrowIfNull(string url)
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url));
        }
    }
}
