using System;
using ExponentialHttpClient;
using ExponentialHttpClient.Exceptions;

namespace ExponentialHttpClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RetryingHttpClient();

            try
            {
                //var a = client.Get<string>("http://somerandomadress.lalala/probablynotalive");
                var a = client.Get<string>("http://52.233.180.248:666/api/httpClientChecks/isalive");
            }
            catch (HttpRequestFailedException ex)
            {
                Console.WriteLine($"Success: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BOO HOO YOU FAILED: {ex.Message}");
            }
        }
    }
}
