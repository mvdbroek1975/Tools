using System.Collections.Specialized;
using System.Configuration;
using System.Net;

namespace CurrencyPriceChecker.Helpers
{
    public interface IMessagePusher
    {
        void Push(string message);
    }

    public class MessagePusher : IMessagePusher
    {
        private const string PushOverUri = "https://api.pushover.net/1/messages.json";

        public void Push(string message)
        {
            var appToken = ConfigurationManager.AppSettings["AppToken"];
            var userKey = ConfigurationManager.AppSettings["UserKey"];
            var parameters = new NameValueCollection
                             {
                                 {"token", appToken},
                                 {"user", userKey},
                                 {"message", message}
                             };

            using (var client = new WebClient())
            {
                client.UploadValues(PushOverUri, parameters);
            }
        }
    }
}