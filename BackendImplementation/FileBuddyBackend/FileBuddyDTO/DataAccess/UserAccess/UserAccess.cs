using SharedRessources.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedRessources.DataAccess.UserAccess
{
    public class UserAccess : IUserAccess
    {
        private static readonly log4net.ILog Log =
         log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Fetching files on start of client.
        /// </summary>
        /// <param name="userHashId"></param>
        /// <returns></returns>
        public async Task<List<SharedFile>> FetchAvailableFiles(string userHashId)
        {
            return default;
        }

        public async Task<bool> UpdateUserInformation(FullUserData user)
        {
            return default;
        }

        public async Task<FullUserData> GetUserInformation(string userHashId)
        {
            return default;
        }

        public async Task<bool> UpdateGroupInformationOfUser(string userHashId, IList<UserGroup> groups)
        {
            return default;
        }

        public async Task<List<UserGroup>> GetGroupInformationOfUser(string userHashId)
        {
            return default;
        }

        public async Task<IList<FullUserData>> LoadAllUsersFromDatabase()
        {
            return default;
        }

        public async Task DeleteUser(string userHashId)
        {

        }
    }
}
