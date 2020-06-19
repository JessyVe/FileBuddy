using SharedRessources.Dtos;

namespace FileBuddyUI.Helper
{
    /// <summary>
    /// Holds informations of currently locked in user.
    /// </summary>
    public class UserInformation
    {
        private static UserInformation _instance;

        public static UserInformation Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UserInformation();

                return _instance;
            }
        }

        private UserInformation() { }

        public AppUser CurrentUser { get; set; }
    }
}
