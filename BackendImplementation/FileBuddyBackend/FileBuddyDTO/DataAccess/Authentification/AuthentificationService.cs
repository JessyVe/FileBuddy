using Firebase.Database.Query;
using SharedRessources.Dtos;
using System;
using System.Threading.Tasks;

namespace SharedRessources.DataAccess.Authentification
{
    public class AuthentificationService : FireBaseAccessBase, IAuthentificationService
    {
        public async Task<User> RegisterUser(User user)
        {
            Log.Debug("Attempting to register user.");
            _userHashingEngine.SetHash(user);
            var response = await _firebaseClient.Child($"user/{user.HashId}/userinformation").PostAsync(user);
            return response.Object;
        }

        public Task<User> LoginWithMacAddress(string macAddress, string password)
        {
            Log.Debug("Attempting to login user with mail address.");
            throw new NotImplementedException();
        }

        public Task<User> LoginWithMailAddress(string mailAddress, string password)
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
