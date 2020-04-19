using System.Net.Http;

namespace FileBuddyClient.services
{
    public class ApiConnector
    {
        private static ApiConnector _instance;
        public static ApiConnector Instance => _instance ?? (_instance = new ApiConnector());

        private HttpClient _client;

        private ApiConnector()
        {
            _client = new HttpClient();
        }


    }
}
