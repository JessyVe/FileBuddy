﻿using Microsoft.AspNetCore.Mvc;
using SharedRessources.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedRessources.DataAccess.ApiAccess
{
    public interface IApiClient
    {
        Task<IActionResult> Download(string userId, IList<string> filehashes);
        Task<IList<SharedFile>> FetchAvailableFiles(string userId);
        Task<IList<UserGroup>> GetGroupInformation(string userId);
        Task<FullUserData> LoginWithMacAddress(string macAddress, string password);
        Task<FullUserData> LoginWithMailAddress(string mailAddress, string password);
        Task<FullUserData> RegisterUser(FullUserData user);
        Task<IActionResult> UpdateGroupInformationOfUser(string userId, IList<UserGroup> userGroups);
        Task<IActionResult> UpdateUserInformation(FullUserData user);
        Task<IActionResult> Upload(string userId, string filename, IList<UserGroup> userGroups);
    }
}