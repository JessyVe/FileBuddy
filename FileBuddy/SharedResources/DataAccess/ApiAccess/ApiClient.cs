using Microsoft.AspNetCore.Mvc;
using SharedRessources.DisplayedTypes;
using SharedRessources.Dtos;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SharedRessources.DataAccess.ApiAccess
{
    /// <summary>
    /// Implements methods to access the FileBuddy API.
    /// </summary>
    public class ApiClient : ApiClientBase, IApiClient
    {
        private string _baseAddress = "http://localhost";
        private int _port = 5000;

        private const string AuthentificationControllerPath = "api/Authentification";
        private const string TokenControllerPath = "api/Token";
        private const string FileControllerPath = "api/File";
        private const string UserControllerPath = "api/User";

        private static ApiClient _instance;
        public static ApiClient Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ApiClient();

                return _instance;
            }
        }

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
        /// ensure that it is synchronised over all devices. 
        /// </summary>
        /// <param name="macAddress"></param>
        /// <param name="password"></param>
        public async Task<AppUser> LoginWithMacAddress(string macAddress)
        {
            var requestUrl = $"{AuthentificationControllerPath}/login/macaddress/{macAddress}";
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
        /// ensure that it is synchronised over all devices. 
        /// </summary>
        /// <param name="mailAddress"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<AppUser> LoginWithMailAddress(AppUser user)
        {
            var requestUrl = $"{AuthentificationControllerPath}/login/mailaddress";
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
            var requestUrl = $"{AuthentificationControllerPath}/register";
            var result = await ExecutePostCall<AppUser, AppUser>(requestUrl, user);
            return result;
        }

        /// <summary>
        /// Uploads given files and makes them available for
        /// definited users.
        /// </summary>
        /// <param name="files"></param>
        /// <param name="userGroups"></param>
        /// <returns></returns>
        public async Task Upload(int userId, IList<UserGroup> userGroups, string filePath, string accessToken)
        {
            var requestUrl = $"{FileControllerPath}/upload/{userId}/{userId}"; // TODO: Change to user group
            await ExecuteCallWithMultipartFormDataContent(requestUrl, filePath, accessToken); 
        }

        /// <summary>
        /// Uploads given files and makes them available for
        /// definited users.
        /// </summary>
        /// <param name="files"></param>
        /// <param name="userGroups"></param>
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
        /// <returns></returns>
        public async Task<IActionResult> UpdateUserInformation(AppUser user, string accessToken)
        {
            var requestUrl = $"{UserControllerPath}/update/{user}";
            var result = await ExecuteCall<IActionResult>(requestUrl, accessToken);
            return result;
        }
    }
}