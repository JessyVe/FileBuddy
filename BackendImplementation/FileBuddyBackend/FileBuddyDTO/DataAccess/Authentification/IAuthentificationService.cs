using SharedRessources.Dtos;
using System.Threading.Tasks;

namespace SharedRessources.DataAccess.Authentification
{
    public interface IAuthentificationService
    {
        public Task<FullUserData> RegisterUser(FullUserData user);

        public Task<FullUserData> LoginWithMacAddress(string macAddress, string password);
        public Task<FullUserData> LoginWithMailAddress(string mailAddress, string password);

        public AuthentificationToken RefreshToken(AuthentificationToken authentificationToken);
        public void RevokeToken(string userId);
    }
}
