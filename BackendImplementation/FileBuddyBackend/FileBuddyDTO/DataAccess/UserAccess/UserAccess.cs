using Firebase.Database.Query;
using SharedRessources.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedRessources.DataAccess.UserAccess
{
    public class UserAccess : FireBaseAccessBase, IUserAccess
    {
        /// <summary>
        /// Fetching files on start of client.
        /// </summary>
        /// <param name="userHashId"></param>
        /// <returns></returns>
        public async Task<List<SharedFile>> FetchAvailableFiles(string userHashId)
        {
            var response = await _firebaseClient.Child($"user/{userHashId}/fetchablefiles").OnceAsync<List<SharedFile>>();
            if (response.Count == 1)
            {
                return response.GetEnumerator().Current.Object;
            }
            return new List<SharedFile>();
        }

        public async Task<bool> UpdateUserInformation(User user)
        {
            // TODO: Check if MAC-Address is already assigned to other account
            var response = await _firebaseClient.Child($"user/{user.HashId}/userinformation").PostAsync(user);
            return response != null;
        }

        public async Task<User> GetUserInformation(string userHashId)
        {
            var response = await _firebaseClient.Child($"user/{userHashId}/userinformation").OnceAsync<User>();
            if (response.Count == 1)
                foreach (var firebaseObject in response)
                {
                    return firebaseObject.Object;
                }
            return null;
        }

        public async Task<bool> UpdateGroupInformationOfUser(string userHashId, IList<UserGroup> groups)
        {
            var response = await _firebaseClient.Child($"user/{userHashId}/groups").PostAsync(groups);
            return response != null || response?.Object.Count == 0;
        }

        public async Task<List<UserGroup>> GetGroupInformationOfUser(string userHashId)
        {
            var response = await _firebaseClient.Child($"user/{userHashId}/groups").OnceSingleAsync<List<UserGroup>>();
            return response;
        }
    }
}
