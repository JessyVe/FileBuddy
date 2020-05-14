using SharedRessources.Dtos;
using System.Collections.Generic;

namespace SharedRessources.DataAccess.UserAccess
{
    public interface IUserAccess
    {
        IList<SharedFile> FetchAvailableFiles(int userId);

        bool UpdateUserInformation(AppUser user);
        AppUser GetUserInformation(int userId);


        IList<AppUser> LoadAllUsersFromDatabase();

        bool DeleteUser(int userId);
    }
}
