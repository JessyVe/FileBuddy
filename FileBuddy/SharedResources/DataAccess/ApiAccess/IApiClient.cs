﻿using Microsoft.AspNetCore.Mvc;
using SharedRessources.DisplayedTypes;
using SharedRessources.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedRessources.DataAccess.ApiAccess
{
    public interface IApiClient
    {
        Task<AppUser> LoginWithMacAddress(string macAddress);
        Task<AppUser> LoginWithMailAddress(AppUser user);
        Task<AppUser> RegisterUser(AppUser user);
        Task Upload(int userId, IList<UserGroup> userGroups, string filePath);
        Task<string> Download(DownloadRequest downloadRequest);
        Task<ICollection<DisplayedSharedFile>> FetchAvailableFiles(int userId);
        Task<IActionResult> UpdateUserInformation(AppUser user);     
    }
}