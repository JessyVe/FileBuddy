using SharedRessources.Dtos;

namespace FileBuddyUI
{
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
