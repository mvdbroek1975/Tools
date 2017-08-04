using System;
using CurrencyPriceChecker.Helpers;

namespace CurrencyPriceChecker.Exceptions
{
    public class EutheriumException : Exception
    {
        public EutheriumException(IMessagePusher messagePusher, string message)
            : base(message)
        {
            messagePusher.Push(message);
        }

        public EutheriumException(IMessagePusher messagePusher, Exception inner)
            : base(inner.Message, inner)
        {
            messagePusher.Push(inner.Message);
        }
    }
}