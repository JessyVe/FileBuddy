using Microsoft.AspNetCore.Mvc;
using SharedRessources.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedRessources.DataAccess.ApiAccess
{
    public interface IApiClient
    {
        Task<AppUser> LoginWithMacAddress(AppUser user, string macAddress);
        Task<AppUser> LoginWithMailAddress(AppUser user);
        Task<AppUser> RegisterUser(AppUser user);
        Task<IActionResult> Upload(int userId, IList<UserGroup> userGroups);
        Task<IActionResult> Download(string apiPath);
        Task<IList<SharedFile>> FetchAvailableFiles(string userId);
        Task<IActionResult> UpdateUserInformation(AppUser user);     
    }
}