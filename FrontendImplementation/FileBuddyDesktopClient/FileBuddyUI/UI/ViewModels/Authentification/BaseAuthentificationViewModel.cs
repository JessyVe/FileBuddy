using FileBuddyUI.UI.Helper;
using SharedRessources.Services;
using System;
using ToastNotifications.Messages;

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
                else
                    ToastMessenger.NotifierInstance.ShowWarning(UITexts.InvalidMailAddress);
            }
        }

        public string Password { get; set; }

        public event EventHandler AuthentificationSuccess;

        protected virtual void OnAuthentificationSuccess(AuthentificationEventArgs e)
        {
            EventHandler handler = AuthentificationSuccess;
            handler?.Invoke(this, e);
        }
    }
}
