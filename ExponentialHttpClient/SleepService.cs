using System;
using System.Threading.Tasks;

namespace ExponentialHttpClient
{
    internal class SleepService : ISleepService
    {
        public async Task Sleep(int sleepTimeInMilliseconds)
        {
            await Task.Delay(sleepTimeInMilliseconds);
        }
    }
}
