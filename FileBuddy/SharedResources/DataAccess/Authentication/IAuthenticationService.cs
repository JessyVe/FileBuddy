
using SharedResources.Dtos;

namespace SharedResources.DataAccess.Authentication
{
    public interface IAuthenticationService
    {
        public AppUser RegisterUser(AppUser user);
        public AppUser LoginWithMacAddress(string macAddress);
        public AppUser LoginWithMailAddress(string mailAddress, string password);

        public bool MailAddressAlreadyInUse(string mailAddress);
    }
}
