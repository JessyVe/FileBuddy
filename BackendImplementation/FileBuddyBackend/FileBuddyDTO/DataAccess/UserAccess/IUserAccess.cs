using SharedRessources.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedRessources.DataAccess.UserAccess
{
    public interface IUserAccess
    {
        Task<List<SharedFile>> FetchAvailableFiles(string userHashId);

        Task<bool> UpdateUserInformation(FullUserData user);
        Task<FullUserData> GetUserInformation(string userHashId);

        Task<bool> UpdateGroupInformationOfUser(string userHashId, IList<UserGroup> userGroups);
        Task<List<UserGroup>> GetGroupInformationOfUser(string userHashId);

        Task<IList<FullUserData>> LoadAllUsersFromDatabase();

        Task DeleteUser(string userHashId);
    }
}
