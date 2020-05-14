﻿using FileBuddyUI.UI.Helper;
using SharedRessources.Services;

namespace FileBuddyUI.UI.ViewModels
{
    public class LoginScreenViewModel : ViewModelBase
    {
        private string _mailAddress;
        public string MailAddress
        {
            get => _mailAddress;
            set
            {
                if (DataValidator.IsMailAddressValid(value))
                    _mailAddress = value;

                // TODO: Else show error
            }
        }

        public string Password { get; set; }
    }
}
