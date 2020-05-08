using Firebase.Database.Query;
using SharedRessources.Dtos;
using System;
using System.Threading.Tasks;

namespace SharedRessources.DataAccess.Authentification
{
    public class AuthentificationService : FireBaseAccessBase, IAuthentificationService
    {
        public async Task<FullUserData> RegisterUser(FullUserData user)
        {
            Log.Debug("Attempting to register user.");
            _userHashingEngine.SetHash(user);
            var fullUserDataResponse = await _firebaseClient.Child($"user/{user.HashId}/userinformation").PostAsync(user);
            var fullUserData = fullUserDataResponse.Object;

            if(fullUserData != null)
            {
                // unable to delete these ... 
                await _firebaseClient.Child($"users").PostAsync(new VisibileUserData()
                {
                    Name = fullUserData.Name, 
                    ProfilePicture = fullUserData.ProfilePicture ?? string.Empty
                });
            }
            return fullUserData;
        }

        public Task<FullUserData> LoginWithMacAddress(string macAddress, string password)
        {
            Log.Debug("Attempting to login user with mail address.");
            throw new NotImplementedException();
        }

        public Task<FullUserData> LoginWithMailAddress(string mailAddress, string password)
        {
            Log.Debug("Attempting to login user with mail address.");
            throw new NotImplementedException();
        }

        private AuthentificationToken CreateAuthentificationToken(string userId)
        {
            return TokenGenerator.GenerateAuthentificationToken();
        }

        public AuthentificationToken RefreshToken(AuthentificationToken authentificationToken)
        {
            Log.Debug("Attempting to refresh authentification token.");
            throw new NotImplementedException();
        }

        public void RevokeToken(string userId)
        {
            Log.Debug("Attempting to revoke authentification token.");
            throw new NotImplementedException();
        }
    }
}
