using SharedRessources.DisplayedTypes;
using SharedRessources.Dtos;
using System.Collections.Generic;

namespace SharedRessources.DataAccess.UserAccess
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
