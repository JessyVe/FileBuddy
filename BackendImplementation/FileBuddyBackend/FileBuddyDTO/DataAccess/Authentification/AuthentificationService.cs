using SharedRessources.Dtos;
using System;
using System.Threading.Tasks;

namespace SharedRessources.DataAccess.Authentification
{
    public class AuthentificationService : IAuthentificationService
    {
        private static readonly log4net.ILog Log =
         log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public async Task<FullUserData> RegisterUser(FullUserData user)
        {
            Log.Debug("Attempting to register user.");

            return default;
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
