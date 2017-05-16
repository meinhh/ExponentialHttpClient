using System;
using System.Threading.Tasks;

namespace ExponentialHttpClient
{
    public interface ISleepService
    {
        Task Sleep(int sleepTimeInMilliseconds);
    }
}
