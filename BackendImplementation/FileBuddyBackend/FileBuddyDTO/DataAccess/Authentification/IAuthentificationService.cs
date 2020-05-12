using SharedRessources.Dtos;
using System.Threading.Tasks;

namespace SharedRessources.DataAccess.Authentification
{
    public interface IAuthentificationService
    {
        public Task<AppUser> RegisterUser(AppUser user);

        public Task<AppUser> LoginWithMacAddress(string macAddress, string password);
        public Task<AppUser> LoginWithMailAddress(string mailAddress, string password);

        public AuthentificationToken RefreshToken(AuthentificationToken authentificationToken);
        public void RevokeToken(string userId);
    }
}
