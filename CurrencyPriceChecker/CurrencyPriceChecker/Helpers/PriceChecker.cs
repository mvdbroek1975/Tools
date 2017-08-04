using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using CurrencyPriceChecker.Exceptions;
using CurrencyPriceChecker.Models;

namespace CurrencyPriceChecker.Helpers
{
    public interface IPriceChecker
    {
        double CheckPrice(string currency, double previousPrice, double minimum);
    }

    public class PriceChecker : IPriceChecker
    {

        private readonly IMessagePusher _messagePusher;
        private readonly IApiData<List<CoinMarketCapData>> _apiData;
        private static string Euro => Encoding.Default.GetString(new byte[] { 128 });

        public PriceChecker(IMessagePusher messagePusher, IApiData<List<CoinMarketCapData>> apiData)
        {
            _messagePusher = messagePusher;
            _apiData = apiData;
        }

        public double CheckPrice(string currency, double previousPrice, double minimum)
        {
            var data = _apiData.GetData(currency);
            var bla = data.Single(q => q.symbol == currency.ToUpperInvariant());
            var currentPrice = Math.Round(double.Parse(bla.price_eur), 2);
            var message = $"The current {bla.name} value is {Euro} {currentPrice} ({bla.percent_change_1h}%)";

            Console.ForegroundColor = currentPrice >= previousPrice ? ConsoleColor.Green : ConsoleColor.Red;

            if (currentPrice <= minimum)
            {
                message = $"The {bla.name} price has dropped below the threshold of {minimum}! The current value is {Euro} {currentPrice} ({bla.percent_change_1h}%)";
                _messagePusher.Push(message);
            }

            Console.WriteLine(message);

            return currentPrice;
        }
    }

    public interface IApiData<out T>
    {
        T GetData(string currency);
    }

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
                throw new EutheriumException(_messagePusher, $"{uri} is unreachable!");

            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(json);
            if (data == null)
                throw new EutheriumException(_messagePusher, "Couldn't deserialize market data");

            return data;
        }
    }


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
                throw new EutheriumException(_messagePusher, $"{EtheuriumUri} is unreachable!");

            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CoinMarketCapData>>(json);
            if (data == null)
                throw new EutheriumException(_messagePusher, "Couldn't deserialize market data");

            return data;
        }
    }
}