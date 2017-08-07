using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurrencyPriceChecker.Contracts;
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
}