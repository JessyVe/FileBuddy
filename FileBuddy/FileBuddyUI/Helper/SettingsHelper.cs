using System.Net;

namespace FileBuddyUI.Helper
{
    public class SettingsHelper
    {
        private static SettingsHelper _instance;

        public static SettingsHelper Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SettingsHelper();

                return _instance;
            }
        }

        private SettingsHelper() { }

        public ApplicationSettings ApplicationSettings { get; set; } = new ApplicationSettings()
        {
            SocketServerAddress = IPAddress.Parse("127.0.0.1"),
            SocketServerPort = 8000
        };
    }
}
