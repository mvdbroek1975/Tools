using System.Collections.Generic;
using System.Net;
using CurrencyPriceChecker.Contracts;
using CurrencyPriceChecker.Exceptions;
using CurrencyPriceChecker.Models;

namespace CurrencyPriceChecker.Helpers
{
    public class CoinMarketCapApiData : IApiData<List<CoinMarketCapData>>
    {
        private readonly IMessagePusher _messagePusher;
        private const string EtheuriumUri = "https://api.coinmarketcap.com/v1/ticker/?convert=EUR";

        public CoinMarketCapApiData(IMessagePusher messagePusher)
        {
            _messagePusher = messagePusher;
        }

        public List<CoinMarketCapData> GetData(string currency)
        {
            var json = new WebClient().DownloadString(EtheuriumUri);
            if (string.IsNullOrWhiteSpace(json))
                throw new CryptoCurrencyException(_messagePusher, $"{EtheuriumUri} is unreachable!");

            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CoinMarketCapData>>(json);
            if (data == null)
                throw new CryptoCurrencyException(_messagePusher, "Couldn't deserialize market data");

            return data;
        }
    }
}