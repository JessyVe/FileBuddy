using SharedRessources.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedRessources.DataAccess
{
    public interface IDatabaseAccess
    {
        public Task<User> LoginWithMacAddress(string macAddress, string password);
        public Task<User> LoginWithMailAddress(string mailAddress, string password);

        public Task<User> RegisterUser(User user);
        public Task DeleteUser(string userHashId);

        public Task UploadFiles(string userHashId, IList<SharedFile> filePaths, IList<UserGroup> userGroups);
        public Task DownloadFiles(string[] downloadUrls);

        public Task<IList<SharedFile>> FetchAvailableFiles(string userHashId);

        public Task<bool> UpdateUserInformation(User user);
        public Task<User> GetUserInformation(string userHashId);
   
        public Task<bool> UpdateGroupInformationOfUser(string userHashId, IList<UserGroup> userGroups);
        public Task<IList<UserGroup>> GetGroupInformationOfUser(string userHashId);
    }
}
