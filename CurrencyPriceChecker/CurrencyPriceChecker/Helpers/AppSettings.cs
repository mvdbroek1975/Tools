using System.Collections.Generic;
using System.IO;
using CurrencyPriceChecker.Exceptions;
using CurrencyPriceChecker.Models;

namespace CurrencyPriceChecker.Helpers
{

    public interface IAppSettings
    {
        IEnumerable<Coin> Coins { get; }
    }

    public class AppSettings : IAppSettings
    {
        public AppSettings(IMessagePusher messagePusher)
        {
            var json = File.ReadAllText("settings.json");
            if (string.IsNullOrEmpty(json))
                throw new EutheriumException(messagePusher, "No settings json file found");

            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<TriggersRootObject>(json);
            if (data == null)
                throw new EutheriumException(messagePusher, "Invalid json setting file");

            Coins = data.Triggers.Coins;
        }


        public IEnumerable<Coin> Coins { get; private set; }
    }
}