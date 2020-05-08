using SharedRessources.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedRessources.DataAccess.UserAccess
{
    public interface IUserAccess
    {
        public Task<List<SharedFile>> FetchAvailableFiles(string userHashId);

        public Task<bool> UpdateUserInformation(User user);
        public Task<User> GetUserInformation(string userHashId);

        public Task<bool> UpdateGroupInformationOfUser(string userHashId, IList<UserGroup> userGroups);
        public Task<List<UserGroup>> GetGroupInformationOfUser(string userHashId);
    }
}
