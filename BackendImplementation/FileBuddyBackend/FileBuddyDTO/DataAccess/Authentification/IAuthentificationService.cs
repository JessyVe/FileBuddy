using SharedRessources.Dtos;
using System.Threading.Tasks;

namespace SharedRessources.DataAccess.Authentification
{
    public interface IAuthentificationService
    {
        public Task<User> RegisterUser(User user);

        public Task<User> LoginWithMacAddress(string macAddress, string password);
        public Task<User> LoginWithMailAddress(string mailAddress, string password);

        public AuthentificationToken RefreshToken(AuthentificationToken authentificationToken);
        public void RevokeToken(string userId);
    }
}
