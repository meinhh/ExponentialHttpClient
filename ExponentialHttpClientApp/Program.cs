using System;
using ExponentialHttpClient;
using ExponentialHttpClient.Exceptions;
using Newtonsoft.Json.Linq;

namespace ExponentialHttpClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var link = "http://localhost:666";
            var client = new RetryingHttpClient();

            try
            {
                /*var isIdan = client.Get<bool>($"{link}/isidan/idani").Result;
                Console.WriteLine($"Is alive: {isIdan}");*/

                /*var displayName = client.PostAsJson($"{link}/tvdisplayname", 
                    new JObject{ {"id", "666"}, { "displayName", "avatar: the last airbender" }  }).Result;
                Console.WriteLine($"display name: {displayName}");*/

                var displayName = client.PostAsJson($"{link}/tvdisplayname",
                                    new JObject { { "id", "666" }}).Result;
                Console.WriteLine($"display name: {displayName}");
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
