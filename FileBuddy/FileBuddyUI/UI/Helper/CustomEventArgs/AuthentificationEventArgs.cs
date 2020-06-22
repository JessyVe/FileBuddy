using SharedRessources.Dtos;
using System;

namespace FileBuddyUI.UI.Helper.CustomEventArgs
{
    public class AuthentificationEventArgs : EventArgs
    {
        public AppUser AppUser { get; set; }
        public bool IsNewUser { get; set; }
    }
}
