using System;
using System.Collections.Generic;
using System.Text;
using CurrencyPriceChecker.Helpers;
using CurrencyPriceChecker.Models;
using SimpleInjector;

namespace CurrencyPriceChecker
{
    class Bootstrapper
    {
        public static Container container;

        public static void Boot()
        {
            container = new Container();
            container.Register<IMessagePusher, MessagePusher>();
            container.Register<IAppSettings, AppSettings>();
            container.Register<IApiData<List<CoinMarketCapData>>, CoinMarketCapApiData>();
            container.Register<IApiData<RootObject>, NexUistApiData>();
            container.Register<IPriceChecker, PriceChecker>();
            container.Verify();

            Console.OutputEncoding = Encoding.Default;
        }
    }
}