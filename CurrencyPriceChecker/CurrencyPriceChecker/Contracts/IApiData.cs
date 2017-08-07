namespace CurrencyPriceChecker.Contracts
{
    public interface IApiData<out T>
    {
        T GetData(string currency);
    }
}