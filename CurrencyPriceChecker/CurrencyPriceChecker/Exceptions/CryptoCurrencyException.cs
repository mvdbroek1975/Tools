using System;
using CurrencyPriceChecker.Helpers;

namespace CurrencyPriceChecker.Exceptions
{
    public class CryptoCurrencyException : Exception
    {
        public CryptoCurrencyException(IMessagePusher messagePusher, string message)
            : base(message)
        {
            messagePusher.Push(message);
        }

        public CryptoCurrencyException(IMessagePusher messagePusher, Exception inner)
            : base(inner.Message, inner)
        {
            messagePusher.Push(inner.Message);
        }
    }
}