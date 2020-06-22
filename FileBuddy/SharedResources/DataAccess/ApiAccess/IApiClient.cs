using Microsoft.AspNetCore.Mvc;
using SharedRessources.DisplayedTypes;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharedResources.Dtos;

namespace SharedResources.DataAccess.ApiAccess
{
    public interface IApiClient
    {
        Task<AppUser> LoginWithMacAddress(string macAddress);
        Task<AppUser> LoginWithMailAddress(AppUser user);
        Task<AppUser> RegisterUser(AppUser user);

        Task Upload(int userId, IList<UserGroup> userGroups, string filePath, string accessToken);
        Task<string> Download(DownloadRequest downloadRequest, string accessToken);
        Task<ICollection<DisplayedSharedFile>> FetchAvailableFiles(int userId, string accessToken);
        Task<IActionResult> UpdateUserInformation(AppUser user, string accessToken);
    }
}