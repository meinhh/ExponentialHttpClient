using System;
using System.Threading.Tasks;

namespace ExponentialHttpClient
{
    public class SleepService : ISleepService
    {
        public async Task Sleep(int sleepTimeInMilliseconds)
        {
            await Task.Delay(sleepTimeInMilliseconds);
        }
    }
}
