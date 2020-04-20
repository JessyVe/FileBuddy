using SharedRessources.Dtos;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedRessources.DataAccess
{
    public class ApiClient
    {
        private HttpClient _client = new HttpClient();
        private string _baseAddress;
        private int _port;

        public ApiClient()
        {
            _client.BaseAddress = new Uri($"{_baseAddress}:{_port}/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<User> LoginWithMacAddress(string macAddress, string password)
        {
            var requestUrl = $"login/macaddress/{macAddress}/{password}";
            var result = await ExecuteCall<User>(requestUrl);
            return result;
        }

        private async Task<T> ExecuteCall<T>(string requestUrl)
        {
            using var streamTask = _client.GetStreamAsync(requestUrl);
            var response = await JsonSerializer.DeserializeAsync<T>(await streamTask);
            return response;
        }
    }
}
