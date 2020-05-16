using FileBuddyUI.UI.Helper;
using SharedRessources.DataAccess.ApiAccess;
using SharedRessources.Dtos;
using SharedRessources.Services;
using System;
using System.Windows.Input;
using ToastNotifications.Messages;

namespace FileBuddyUI.UI.ViewModels
{
    public class LoginScreenViewModel : BaseAuthentificationViewModel
    {
        public ICommand OnLogin { get; }
        public ICommand OnLoginWithMac { get; }

        public LoginScreenViewModel()
        {
            OnLogin = new RelayCommand(o => LoginUser());
            OnLoginWithMac = new RelayCommand(o => LoginUserWithMac());
        }      

        private async void LoginUser()
        {
            try
            {
                if (string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(MailAddress))
                {
                    ToastMessenger.NotifierInstance.ShowWarning(UITexts.NoDataLoginError);
                    return;
                }
                else if (string.IsNullOrEmpty(Password))
                {
                    ToastMessenger.NotifierInstance.ShowWarning(UITexts.NoPasswordGiven);
                    return;
                }
                else if (string.IsNullOrEmpty(MailAddress))
                {
                    ToastMessenger.NotifierInstance.ShowWarning(UITexts.NoMailAddressGiven);
                    return;
                }

                var user = new AppUser()
                {
                    MailAddress = MailAddress,
                    Password = Password
                };

                UIService.SetBusyState();
                var loggedInUser = await ApiClient.Instance.LoginWithMailAddress(user);

                if (loggedInUser.Id > 0)
                {                   
                    OnAuthentificationSuccess(new AuthentificationEventArgs()
                    {
                        AppUser = loggedInUser
                    });
                } else
                {
                    ToastMessenger.NotifierInstance.ShowError(UITexts.AuthentificationFailed);
                }
            } catch(Exception ex)
            {
                ToastMessenger.NotifierInstance.ShowError($"{UITexts.ExceptionThrown} ({ex.Message})");
            }
        }

        private async void LoginUserWithMac()
        {
            try
            {
                UIService.SetBusyState();
                var loggedInUser = await ApiClient.Instance.LoginWithMacAddress(MacAddressRetriever.GetMacAddress());
                if (loggedInUser.Id > 0)
                {
                    OnAuthentificationSuccess(new AuthentificationEventArgs()
                    {
                        AppUser = loggedInUser
                    });
                }
                else
                {
                    ToastMessenger.NotifierInstance.ShowError(UITexts.AuthentificationFailed);
                }
            }
            catch (Exception ex)
            {
                ToastMessenger.NotifierInstance.ShowError($"{UITexts.ExceptionThrown} ({ex.Message})");
            }
        }
    }
}
