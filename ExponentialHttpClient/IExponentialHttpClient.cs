using System;
using System.Threading.Tasks;

namespace ExponentialHttpClient
{
    public interface IExponentialHttpClient
    {
        Task<TDestinationType> PostAsJson<TSourceType, TDestinationType>(string url, TSourceType data);

        Task<string> PostAsJson<TSourceType>(string url, TSourceType data);
        
        Task<TDestinationType> Get<TDestinationType>(string url);
    }
}
