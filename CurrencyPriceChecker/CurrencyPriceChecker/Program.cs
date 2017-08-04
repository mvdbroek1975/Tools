using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CurrencyPriceChecker.Helpers;
using CurrencyPriceChecker.Models;

namespace CurrencyPriceChecker
{
    internal static class Program
    {
        private static void Main()
        {
            Bootstrapper.Boot();

            var settings = Bootstrapper.container.GetInstance<IAppSettings>();

            foreach (var coin in settings.Coins)
            {
                Task.Factory.StartNew(() => StartChecker(coin));
            }

            Console.ReadLine();
        }


        private static void StartChecker(Coin coin)
        {
            var checker = Bootstrapper.container.GetInstance<IPriceChecker>();
            var previousPrice = 0.00;
            var thresHold = double.Parse(coin.Threshold);
            var interval = int.Parse(coin.Interval);

            while (true)
            {
                previousPrice = checker.CheckPrice(coin.Currency, previousPrice, thresHold);
                Thread.Sleep(interval);
            }
        }
    }
}
