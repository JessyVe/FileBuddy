using Microsoft.AspNetCore.Mvc;
using SharedRessources.Dtos;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SharedRessources.DataAccess
{
    /// <summary>
    /// Implements methods to access the FileBuddy API
    /// </summary>
    public class ApiClient : ApiClientBase, IApiClient
    {
        // TODO: Extract into configuration
        private string _baseAddress = "https://localhost";
        private int _port = 5001;

        public string ControllerPath { get; set; }

        public ApiClient()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri($"{_baseAddress}:{_port}")
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            ControllerPath = "api/filebuddy";
        }

        /// <summary>
        /// Returns the current user object to 
        /// ensure that it is synchronised over all devices. 
        /// </summary>
        /// <param name="macAddress"></param>
        /// <param name="password"></param>
        public async Task<User> LoginWithMacAddress(string macAddress, string password)
        {
            var requestUrl = $"{ControllerPath}login/macaddress/{macAddress}/{password}";
            var result = await ExecuteCall<User>(requestUrl);
            return result;
        }

        /// <summary>
        /// Returns the current user object to 
        /// ensure that it is synchronised over all devices. 
        /// </summary>
        /// <param name="mailAddress"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<User> LoginWithMailAddress(string mailAddress, string password)
        {
            var requestUrl = $"{ControllerPath}login/macaddress/{mailAddress}/{password}";
            var result = await ExecuteCall<User>(requestUrl);
            return result;
        }

        /// <summary>
        /// Returns the user object with the assigned 
        /// user id needed for further transactions.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<User> RegisterUser(User user)
        {
            var requestUrl = $"{ControllerPath}/register";
            var result = await ExecutePostCall<User, User>(requestUrl, user);
            return result;
        }

        /// <summary>
        /// Uploads given files and makes them available for
        /// definited users.
        /// </summary>
        /// <param name="files"></param>
        /// <param name="userGroups"></param>
        /// <returns></returns>
        public async Task<IActionResult> Upload(string userId, string filename, IList<UserGroup> userGroups)
        {
            var requestUrl = $"{ControllerPath}upload/{filename}/{userId}/{userGroups}";
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
        public async Task<IActionResult> Download(string userId, IList<string> filehashes)
        {
            var requestUrl = $"{ControllerPath}download/{userId}/{filehashes}";
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
            var requestUrl = $"{ControllerPath}fetch/files/{userId}";
            var result = await ExecuteCall<IList<SharedFile>>(requestUrl);
            return result;
        }

        /// <summary>
        /// Updates user-specific settings.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IActionResult> UpdateUserInformation(User user)
        {
            var requestUrl = $"{ControllerPath}update/user/{user}";
            var result = await ExecuteCall<IActionResult>(requestUrl);
            return result;
        }

        /// <summary>
        /// Updates the group informations for an user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="jsonGroupInformation"></param>
        /// <returns></returns>
        public async Task<IActionResult> UpdateGroupInformationOfUser(string userId, IList<UserGroup> userGroups)
        {
            var requestUrl = $"{ControllerPath}update/groups/{userId}/{userGroups}";
            var result = await ExecuteCall<IActionResult>(requestUrl);
            return result;
        }

        /// <summary>
        /// Returns a a collection containing user groups 
        /// created by the user. With this method user groups
        /// can be synchronized over all devices.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="jsonGroupInformation"></param>
        /// <returns></returns>
        public async Task<IList<UserGroup>> GetGroupInformation(string userId)
        {
            var requestUrl = $"{ControllerPath}fetch/groups/{userId}";
            var result = await ExecuteCall<IList<UserGroup>>(requestUrl);
            return result;
        }
    }
}