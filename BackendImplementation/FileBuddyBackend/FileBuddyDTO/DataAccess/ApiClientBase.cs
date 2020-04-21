using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedRessources.DataAccess
{
    public abstract class ApiClientBase
    {
        protected HttpClient _client;

        /// <summary>
        /// Executes a call to the given url and returns 
        /// the result of the API.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        protected async Task<T> ExecuteCall<T>(string requestUrl)
        {
            using var streamTask = _client.GetStreamAsync(requestUrl);
            var response = await JsonSerializer.DeserializeAsync<T>(await streamTask);
            return response;
        }

        protected async Task<TResponse> ExecutePostCall<TRequest, TResponse>(string requestUrl, TRequest contentObject)
        {
            var response = await _client.PostAsJsonAsync(requestUrl, contentObject);
            response.EnsureSuccessStatusCode();

            return await GetResponseOrError<TResponse>(response);
        }

        private async Task<T> GetResponseOrError<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var item = await response.Content.ReadAsAsync<T>();
                    return item;
                }
                catch (Exception ex)
                {
                    // TODO: Log
                    throw;
                }
            }

            throw await GetException(response);
        }

        private async Task<Exception> GetException(HttpResponseMessage response)
        {
            return new Exception();
        }
    }
}
