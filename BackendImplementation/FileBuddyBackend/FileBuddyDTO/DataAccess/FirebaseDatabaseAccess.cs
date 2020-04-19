using Firebase.Database;
using Firebase.Database.Query;
using SharedRessources.Dtos;
using SharedRessources.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedRessources.DataAccess
{
    public class FirebaseDatabaseAccess : IDatabaseAccess
    {
        private static readonly log4net.ILog Log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private FirebaseClient _firebaseClient;

        // TODO: Extract to settings
        private const string AuthSecret = "G9gFvZ3Jz4DKHczBMRNfRC7iNM2XWREyesbbk6kx";
        private const string BasePath = "https://filebuddy-403f3.firebaseio.com/";

        private HashingEngine _userHashingEngine;
        private HashingEngine _fileHashingEngine;

        public FirebaseDatabaseAccess()
        {
            InitializeFirebaseClient();
            _userHashingEngine = new UserHashingEngine();
            _fileHashingEngine = new FileHashingEngine();
        }

        private void InitializeFirebaseClient()
        {
            Log.Debug("Initializing Firebase client ...");
            try
            {
                _firebaseClient = new FirebaseClient(
                  BasePath,
                  new FirebaseOptions
                  {
                      AuthTokenAsyncFactory = () => Task.FromResult(AuthSecret)
                  });
            }
            catch (Exception ex)
            {
                Log.Error("Initialization of Firebase client failed!", ex);
            }
            Log.Debug("Finished initialization of Firebase client!");
        }

        public async Task<User> LoginWithMacAddress(string macAddress, string password)
        {
            //var result = await _firebaseClient.Child($"fetch/files/{userHashId}");
            throw new System.NotImplementedException();
        }

        public async Task<User> LoginWithMailAddress(string mailAddress, string password)
        {
            //var result = await _firebaseClient.Child($"fetch/files/{userHashId}");
            throw new System.NotImplementedException();
        }

        public async Task<User> RegisterUser(User user)
        {
            _userHashingEngine.SetHash(user);
            var response = await _firebaseClient.Child($"user/{user.HashId}/userinformation").PostAsync(user);
            return response.Object;
        }

        public async Task DeleteUser(string userHashId)
        {
            await _firebaseClient.Child($"user").Child(userHashId).DeleteAsync();
        }

        public Task UploadFiles(string userHashId, IList<SharedFile> filePaths, IList<UserGroup> userGroups)
        {
            //TODO: Set information in database
            throw new System.NotImplementedException();
        }

        public Task DownloadFiles(string[] downloadUrls)
        {
            // TODO: set information in database
            throw new System.NotImplementedException();
        }

        public async Task<IList<SharedFile>> FetchAvailableFiles(string userHashId)
        {
            var response = await _firebaseClient.Child($"fetch/files/{userHashId}").OnceAsync<IList<SharedFile>>();
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
            if(response.Count == 1)
                foreach(var firebaseObject in response)
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

        public async Task<IList<UserGroup>> GetGroupInformationOfUser(string userHashId)
        {
            var response = await _firebaseClient.Child($"user/{userHashId}/groups").OnceSingleAsync<IList<UserGroup>>();
            return response;
        }
    }
}