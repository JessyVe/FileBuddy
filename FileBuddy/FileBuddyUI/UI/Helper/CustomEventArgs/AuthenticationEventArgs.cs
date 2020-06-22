using System;
using SharedResources.Dtos;

namespace FileBuddyUI.UI.Helper.CustomEventArgs
{
    public class AuthenticationEventArgs : EventArgs
    {
        public AppUser AppUser { get; set; }
        public bool IsNewUser { get; set; }
    }
}
