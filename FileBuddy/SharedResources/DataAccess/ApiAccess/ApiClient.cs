using Microsoft.AspNetCore.Mvc;
using SharedResources.DisplayedTypes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SharedResources.Dtos;

namespace SharedResources.DataAccess.ApiAccess
{
    /// <summary>
    /// Implements methods to access the FileBuddy API.
    /// </summary>
    public class ApiClient : ApiClientBase, IApiClient
    {
        private readonly string _baseAddress = "http://localhost";
        private readonly int _port = 5000;

        private const string AuthenticationControllerPath = "api/Authentication";
        private const string TokenControllerPath = "api/Token";
        private const string FileControllerPath = "api/File";
        private const string UserControllerPath = "api/User";

        private static ApiClient _instance;
        public static ApiClient Instance => _instance ??= new ApiClient();

        private ApiClient()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri($"{_baseAddress}:{_port}"),
            };
            _client.DefaultRequestHeaders.Accept.Clear();
        }

        /// <summary>
        /// Returns the current user object to 
        /// ensure that it is synchronized over all devices. 
        /// </summary>
        /// <param name="macAddress"></param>
        public async Task<AppUser> LoginWithMacAddress(string macAddress)
        {
            var requestUrl = $"{AuthenticationControllerPath}/login/macaddress/{macAddress}";
            var result = await ExecuteCall<AppUser>(requestUrl);
            return result;
        }

        public Task<AppUser> RefreshAccessToken(AppUser loggedInUser)
        {
            var requestUrl = $"{TokenControllerPath}/refresh/{loggedInUser}";
            //var result = await ExecuteCall<AppUser>(requestUrl, loggedInUser);
            return default;
        }

        /// <summary>
        /// Returns the current user object to 
        /// ensure that it is synchronized over all devices. 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<AppUser> LoginWithMailAddress(AppUser user)
        {
            var requestUrl = $"{AuthenticationControllerPath}/login/mailaddress";
            var result = await ExecutePostCall<AppUser, AppUser>(requestUrl, user);
            return result;
        }

        /// <summary>
        /// Returns the user object with the assigned 
        /// user id needed for further transactions.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<AppUser> RegisterUser(AppUser user)
        {
            var requestUrl = $"{AuthenticationControllerPath}/register";
            var result = await ExecutePostCall<AppUser, AppUser>(requestUrl, user);
            return result;
        }

        /// <summary>
        /// Uploads given files and makes them available for
        /// defined users.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userGroups"></param>
        /// <param name="filePath"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task Upload(int userId, IList<UserGroup> userGroups, string filePath, string accessToken)
        {
            var requestUrl = $"{FileControllerPath}/upload/{userId}/{userId}"; // TODO: Change to user group
            await ExecuteCallWithMultipartFormDataContent(requestUrl, filePath, accessToken); 
        }

        /// <summary>
        /// Uploads given files and makes them available for
        /// defined users.
        /// </summary>
        /// <param name="downloadRequest"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<string> Download(DownloadRequest downloadRequest, string accessToken)
        {
            var requestUrl = $"{FileControllerPath}/download";
            return await DownloadFile(requestUrl, downloadRequest, accessToken);
        }

        /// <summary>
        /// Returns a dictionary containing all available files for the user. 
        /// (Key=userId of sender; Value=list of file names)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<ICollection<DisplayedSharedFile>> FetchAvailableFiles(int userId, string accessToken)
        {
            var requestUrl = $"{UserControllerPath}/fetchfiles/{userId}";
            var result = await ExecuteCall<ICollection<DisplayedSharedFile>>(requestUrl, accessToken);
            return result;
        }

        /// <summary>
        /// Updates user-specific settings.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<IActionResult> UpdateUserInformation(AppUser user, string accessToken)
        {
            var requestUrl = $"{UserControllerPath}/update/{user}";
            var result = await ExecuteCall<IActionResult>(requestUrl, accessToken);
            return result;
        }
    }
}