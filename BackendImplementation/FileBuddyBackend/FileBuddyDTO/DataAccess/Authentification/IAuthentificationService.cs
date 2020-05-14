using SharedRessources.Dtos;

namespace SharedRessources.DataAccess.Authentification
{
    public interface IAuthentificationService
    {
        public AppUser RegisterUser(AppUser user);
        public AppUser LoginWithMacAddress(string macAddress);
        public AppUser LoginWithMailAddress(string mailAddress, string password);


        public AuthentificationToken RefreshToken(AuthentificationToken authentificationToken);
        public void RevokeToken(string userId);
    }
}
