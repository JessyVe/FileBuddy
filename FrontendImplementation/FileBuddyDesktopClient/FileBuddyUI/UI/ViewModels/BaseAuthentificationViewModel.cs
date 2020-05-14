using FileBuddyUI.UI.Helper;
using SharedRessources.Services;
using System;

namespace FileBuddyUI.UI.ViewModels
{
    public abstract class BaseAuthentificationViewModel : ViewModelBase
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

        public event EventHandler AuthentificationSuccess;

        protected virtual void OnAuthentificationSuccess(EventArgs e)
        {
            EventHandler handler = AuthentificationSuccess;
            handler?.Invoke(this, e);
        }
    }
}
