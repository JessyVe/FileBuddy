using SharedResources.DisplayedTypes;
using System.Collections.Generic;
using SharedResources.Dtos;

namespace SharedResources.DataAccess.UserAccess
{
    public interface IUserAccess
    {
        ICollection<DisplayedSharedFile> FetchAvailableFiles(int userId);

        AppUser UpdateUserInformation(AppUser user);
        AppUser GetUserInformation(int userId);


        IList<AppUser> LoadAllUsersFromDatabase();

        bool DeleteUser(int userId);
    }
}
