using System.Security;

namespace FileBuddyUI.UI.Models
{
    public class MainWindowModel
    {
        public string EmailAddress { get; set; }
        public SecureString Password { get; set; }
    }
}
