using System.Net;
using CurrencyPriceChecker.Contracts;
using CurrencyPriceChecker.Exceptions;
using CurrencyPriceChecker.Models;

namespace CurrencyPriceChecker.Helpers
{
    public class NexUistApiData : IApiData<RootObject>
    {
        private readonly IMessagePusher _messagePusher;
        private const string EtheuriumUri = "https://coinmarketcap-nexuist.rhcloud.com/api/{0}";


        public NexUistApiData(IMessagePusher messagePusher)
        {
            _messagePusher = messagePusher;
        }

        public RootObject GetData(string currency)
        {
            var uri = string.Format(EtheuriumUri, currency);
            var json = new WebClient().DownloadString(uri);
            if (string.IsNullOrWhiteSpace(json))
                throw new CryptoCurrencyException(_messagePusher, $"{uri} is unreachable!");

            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(json);
            if (data == null)
                throw new CryptoCurrencyException(_messagePusher, "Couldn't deserialize market data");

            return data;
        }
    }
}