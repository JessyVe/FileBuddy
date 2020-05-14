using SharedRessources.Dtos;
using System;

namespace FileBuddyUI.UI.Helper
{
    public class AuthentificationEventArgs : EventArgs
    {
        public AppUser AppUser { get; set; }
        public bool RegisteredAsNewUser { get; set; }
    }
}
