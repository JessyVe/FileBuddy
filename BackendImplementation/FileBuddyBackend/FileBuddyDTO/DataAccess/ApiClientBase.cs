﻿using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedRessources.DataAccess
{
    /// <summary>
    /// Implements the basic structure for sending requests to 
    /// an API and processing the responses.
    /// </summary>
    public abstract class ApiClientBase
    {
        protected HttpClient _client;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

        /// <summary>
        /// Exceutes a post request to the API 
        /// and returns the result. 
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="requestUrl"></param>
        /// <param name="contentObject"></param>
        /// <returns></returns>
        protected async Task<TResponse> ExecutePostCall<TRequest, TResponse>(string requestUrl, TRequest contentObject)
        {
            var response = await _client.PostAsJsonAsync(requestUrl, contentObject);
            response.EnsureSuccessStatusCode();

            return await GetResponseOrError<TResponse>(response);
        }

        /// <summary>
        /// Returns neither the received data or 
        /// throws an error if the request failed. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
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
                    Log.ErrorFormat("An error occured while reading the response data. ", ex);
                    throw;
                }
            }
            var errorMessage = $"Request was unsuccessful. Statuscode {response.StatusCode} was received. ";
            Log.Error(errorMessage);
            throw new Exception(errorMessage);
        }
    }
}
