using System.Net;

namespace FileBuddyUI.Helper
{
    /// <summary>
    /// Representens the needed client settings. 
    /// </summary>
    public class ApplicationSettings
    {
        public IPAddress SocketServerAddress { get; set; }
        public int SocketServerPort { get; set; }
    }
}
