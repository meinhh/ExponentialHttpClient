using System;

namespace ExponentialHttpClient
{
    [Serializable]
    internal class ClientShouldRetryException : Exception
    {
        public ClientShouldRetryException()
        {
        }

        public ClientShouldRetryException(string message) : base(message)
        {
        }

        public ClientShouldRetryException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}