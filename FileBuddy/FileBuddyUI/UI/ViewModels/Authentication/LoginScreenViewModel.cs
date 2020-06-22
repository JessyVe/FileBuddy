using FileBuddyUI.Resources;
using FileBuddyUI.UI.Helper;
using FileBuddyUI.UI.Helper.CustomEventArgs;
using SharedRessources.Services;
using System;
using System.Windows.Input;
using SharedResources.DataAccess.ApiAccess;
using SharedResources.Dtos;
using ToastNotifications.Messages;

namespace FileBuddyUI.UI.ViewModels.Authentication
{
    /// <summary>
    /// Implements interaction logic for the login view. 
    /// </summary>
    public class LoginScreenViewModel : BaseAuthentificationViewModel
    {
        public ICommand OnLoginWithMailAddress { get; }
        public ICommand OnLoginWithMac { get; }

        public LoginScreenViewModel()
        {
            OnLoginWithMailAddress = new RelayCommand(o => LoginUserWithMailAddress());
            OnLoginWithMac = new RelayCommand(o => LoginUserWithMac());
        }

        private async void LoginUserWithMailAddress()
        {
            BusyCursorProvider.SetBusyState();
            Log.Debug("Attempting to login user with given credentials.");
            try
            {
                if (!ValidateInputData())
                    return;

                var user = new AppUser()
                {
                    MailAddress = MailAddress,
                    Password = Password
                };

                var loggedInUser = await ApiClient.Instance.LoginWithMailAddress(user);
                ValidateLoginResult(loggedInUser);

            }
            catch (Exception ex)
            {
                var errorMessage = $"{UITexts.ExceptionThrown} ({ex.Message})";
                ToastMessenger.NotifierInstance.Notifier.ShowError(errorMessage);
                Log.Debug(errorMessage);
            }
        }

        private bool ValidateInputData()
        {
            if (string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(MailAddress))
            {
                ToastMessenger.NotifierInstance.Notifier.ShowWarning(UITexts.NoDataLoginError);
                Log.Debug(UITexts.NoDataLoginError);
                return false;
            }
            if (string.IsNullOrEmpty(Password))
            {
                ToastMessenger.NotifierInstance.Notifier.ShowWarning(UITexts.NoPasswordGiven);
                Log.Debug(UITexts.NoPasswordGiven);
                return false;
            }
            if (string.IsNullOrEmpty(MailAddress))
            {
                ToastMessenger.NotifierInstance.Notifier.ShowWarning(UITexts.NoMailAddressGiven);
                Log.Debug(UITexts.NoMailAddressGiven);
                return false;
            }
            return true;
        }

        private async void LoginUserWithMac()
        {
            ToastMessenger.NotifierInstance.Notifier.ShowError(UITexts.OnlyForPremiumUser); // TODO: Remove after finished implementation

            BusyCursorProvider.SetBusyState();
            Log.Debug("Attempting to login user with mac address.");
            try
            {
                var loggedInUser = await ApiClient.Instance.LoginWithMacAddress(MacAddressRetriever.GetMacAddress());
                ValidateLoginResult(loggedInUser);
            }
            catch (Exception ex)
            {
                var errorMessage = $"{UITexts.ExceptionThrown} ({ex.Message})";
                ToastMessenger.NotifierInstance.Notifier.ShowError(errorMessage);
                Log.Debug(errorMessage);
            }
        }

        private void ValidateLoginResult(AppUser user)
        {
            if (user.Id > 0)
            {
                OnAuthentificationSuccess(new AuthenticationEventArgs()
                {
                    AppUser = user
                });
            }
            else
            {
                ToastMessenger.NotifierInstance.Notifier.ShowError(UITexts.AuthentificationFailed);
            }
        }
    }
}
