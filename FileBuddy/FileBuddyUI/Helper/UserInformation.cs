using SharedResources.Dtos;

namespace FileBuddyUI.Helper
{
    /// <summary>
    /// Holds the information of currently logged in user.
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
