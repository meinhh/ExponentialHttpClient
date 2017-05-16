using System;
using System.Threading.Tasks;

namespace ExponentialHttpClient
{
    internal interface ISleepService
    {
        Task Sleep(int sleepTimeInMilliseconds);
    }
}
