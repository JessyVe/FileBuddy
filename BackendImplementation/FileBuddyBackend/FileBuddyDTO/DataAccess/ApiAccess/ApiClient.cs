using Microsoft.AspNetCore.Mvc;
using SharedRessources.Dtos;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SharedRessources.DataAccess.ApiAccess
{
    /// <summary>
    /// Implements methods to access the FileBuddy API
    /// </summary>
    public class ApiClient : ApiClientBase, IApiClient
    {
        // TODO: Inject all privates as configuration
        // TODO: Extract into configuration
        private string _baseAddress = "https://localhost";
        private int _port = 5001;

        // TODO: Extract into resource files?
        private const string AuthentificationControllerPath = "api/AuthentificationController/";
        private const string FileControllerPath = "api/FileController/";
        private const string UserControllerPath = "api/UserController/";

        public ApiClient()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri($"{_baseAddress}:{_port}"),
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Returns the current user object to 
        /// ensure that it is synchronised over all devices. 
        /// </summary>
        /// <param name="macAddress"></param>
        /// <param name="password"></param>
        public async Task<AppUser> LoginWithMacAddress(AppUser user, string macAddress)
        {
            var requestUrl = $"{AuthentificationControllerPath}login/macaddress/{macAddress}";
            var result = await ExecutePostCall<AppUser, AppUser>(requestUrl, user);
            return result;
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
            var requestUrl = $"{AuthentificationControllerPath}login/mailaddress";
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
        public async Task<IActionResult> Upload(int userId, IList<UserGroup> userGroups)
        {
            var requestUrl = $"{FileControllerPath}upload/{userId}/{userGroups}";
            var result = await ExecuteCall<IActionResult>(requestUrl);
            return result;
        }

        /// <summary>
        /// Uploads given files and makes them available for
        /// definited users.
        /// </summary>
        /// <param name="files"></param>
        /// <param name="userGroups"></param>
        /// <returns></returns>
        public async Task<IActionResult> Download(string apiPath)
        {
            var requestUrl = $"{FileControllerPath}download/{apiPath}";
            var result = await ExecuteCall<IActionResult>(requestUrl);
            return result;
        }

        /// <summary>
        /// Returns a dictionary containing all available files for the user. 
        /// (Key=userId of sender; Value=list of file names)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IList<SharedFile>> FetchAvailableFiles(string userId)
        {
            var requestUrl = $"{UserControllerPath}fetchfiles/{userId}";
            var result = await ExecuteCall<IList<SharedFile>>(requestUrl);
            return result;
        }

        /// <summary>
        /// Updates user-specific settings.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IActionResult> UpdateUserInformation(AppUser user)
        {
            var requestUrl = $"{UserControllerPath}update/{user}";
            var result = await ExecuteCall<IActionResult>(requestUrl);
            return result;
        }
    }
}