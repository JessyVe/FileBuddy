using FileBuddyUI.Resources;
using FileBuddyUI.UI.Helper;
using FileBuddyUI.UI.Helper.CustomEventArgs;
using SharedRessources.Services;
using System;
using ToastNotifications.Messages;

namespace FileBuddyUI.UI.ViewModels
{
    /// <summary>
    /// Contains basic authentification implentations.
    /// </summary>
    public abstract class BaseAuthentificationViewModel : PropertyChangedBase
    {
        protected static readonly log4net.ILog Log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string _mailAddress;
        public string MailAddress
        {
            get => _mailAddress;
            set
            {
                if (DataValidator.IsMailAddressValid(value))
                    _mailAddress = value;
                else
                    ToastMessenger.NotifierInstance.Notifier.ShowWarning(UITexts.InvalidMailAddress);
            }
        }

        public string Password { get; set; }

        /// <summary>
        /// Is fired when authentification was successfull. 
        /// </summary>
        public event EventHandler AuthentificationSuccess;
        protected virtual void OnAuthentificationSuccess(AuthentificationEventArgs e)
        {
            Log.Debug("AuthentificationSuccess event was invoked.");
            EventHandler handler = AuthentificationSuccess;
            handler?.Invoke(this, e);
        }
    }
}
