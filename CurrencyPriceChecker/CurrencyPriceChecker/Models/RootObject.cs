namespace CurrencyPriceChecker.Models
{
    public class RootObject
    {
        public string symbol { get; set; }
        public string position { get; set; }
        public string name { get; set; }
        public MarketCap market_cap { get; set; }
        public Price price { get; set; }
        public string supply { get; set; }
        public Volume volume { get; set; }
        public string change { get; set; }
        public string timestamp { get; set; }
    }
}