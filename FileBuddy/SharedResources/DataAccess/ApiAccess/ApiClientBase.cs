﻿using SharedResources.Dtos;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedResources.DataAccess.ApiAccess
{
    /// <summary>
    /// Implements the basic structure for sending requests to 
    /// an API and processing the responses.
    /// </summary>
    public abstract class ApiClientBase
    {
        private static readonly log4net.ILog Log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string _destination;
        protected HttpClient _client;

        protected ApiClientBase()
        {
            _destination = Path.Combine(Directory.GetCurrentDirectory(), "Downloads");
            Directory.CreateDirectory(_destination);
        }

        /// <summary>
        /// Executes a call to the given url, using the given access token,
        /// and returns the result of the API.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        protected async Task<T> ExecuteCall<T>(string requestUrl, string token = "")
        {
            if (!string.IsNullOrEmpty(token))
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var streamTask = _client.GetStreamAsync(requestUrl);
            var response = await JsonSerializer.DeserializeAsync<T>(await streamTask);
            return response;
        }

        /// <summary>
        /// Executes a post request to the API, using the given access token, 
        /// and returns the result. 
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="requestUrl"></param>
        /// <param name="contentObject"></param>
        /// <returns></returns>
        protected async Task<TResponse> ExecutePostCall<TRequest, TResponse>(string requestUrl, TRequest contentObject, string token = "")
        {
            if (!string.IsNullOrEmpty(token))
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PostAsJsonAsync(requestUrl, contentObject);
            return await GetResponseOrError<TResponse>(response);
        }

        /// <summary>
        /// Executes an API call and uploads given file. 
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        protected async Task<string> ExecuteCallWithMultipartFormDataContent(string requestUrl, string filePath, string token)
        {
            if (!string.IsNullOrEmpty(token))
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var form = new MultipartFormDataContent();
            HttpContent content = new StringContent("fileToUpload");
            form.Add(content, "fileToUpload");

            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                content = new StreamContent(stream);
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "fileToUpload",
                    FileName = Path.GetFileName(filePath)
                };
                form.Add(content);

                var response = await _client.PostAsync(requestUrl, form);
                return await GetResponseOrError<string>(response);
            }
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

        /// <summary>
        /// Returns the local path to the downloaded file.
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <param name="downloadRequest"></param>
        /// <returns></returns>
        public async Task<string> DownloadFile(string requestUrl, DownloadRequest downloadRequest, string token)
        {
            if (!string.IsNullOrEmpty(token))
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PostAsJsonAsync(requestUrl, downloadRequest);
            var data = response.Content.ReadAsStreamAsync().Result;

            var mailMessage = new MailMessage();
            mailMessage.Attachments.Add(new Attachment(data, response.Content.Headers.ContentDisposition.FileName));

            var filePath = Path.Combine(_destination, response.Content.Headers.ContentDisposition.FileName);
            await using (var fs = File.Create(filePath))
            {
                data.Seek(0, SeekOrigin.Begin);
                data.CopyTo(fs);
            }
            return filePath;
        }
    }
}
