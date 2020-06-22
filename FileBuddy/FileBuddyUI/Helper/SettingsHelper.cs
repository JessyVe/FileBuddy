using System.Net;

namespace FileBuddyUI.Helper
{
    /// <summary>
    /// Provides a centeral location for managing
    /// current settings. 
    /// (NOTE: DEFAULT VALUES SET FROM DEMONSTRATION PRUPOSES)
    /// </summary>
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
            // TODO: Load settings from acutal settings file. 
            SocketServerAddress = IPAddress.Parse("127.0.0.1"),
            SocketServerPort = 8000
        };
    }
}
