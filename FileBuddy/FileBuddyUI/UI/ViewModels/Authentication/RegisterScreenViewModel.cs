using FileBuddyUI.Resources;
using FileBuddyUI.UI.Helper;
using FileBuddyUI.UI.Helper.CustomEventArgs;
using System;
using System.Windows.Input;
using SharedResources.DataAccess.ApiAccess;
using SharedResources.Dtos;
using ToastNotifications.Messages;

namespace FileBuddyUI.UI.ViewModels.Authentication
{
    /// <summary>
    /// Implements interaction logic for the registration view. 
    /// </summary>
    public class RegisterScreenViewModel : BaseAuthentificationViewModel
    {
        public string Username { get; set; }
        public ICommand OnRegister { get; set; }

        public RegisterScreenViewModel()
        {
            OnRegister = new RelayCommand(o => RegisterUser());
        }

        private async void RegisterUser()
        {
            BusyCursorProvider.SetBusyState();
            try
            {
                if (!ValidateInputData())
                    return;

                var user = new AppUser()
                {
                    Name = Username,
                    MailAddress = MailAddress,
                    Password = Password,
                    AccountCreationDate = DateTime.Now
                };
               
                var loggedInUser = await ApiClient.Instance.RegisterUser(user);
                if (loggedInUser.Id > 0)
                {
                    OnAuthentificationSuccess(new AuthenticationEventArgs()
                    {
                        AppUser = loggedInUser,
                        IsNewUser = true
                    });
                }
                else
                {
                    ToastMessenger.NotifierInstance.Notifier.ShowError(UITexts.AuthentificationFailed);
                }
            }
            catch (Exception ex)
            {
                ToastMessenger.NotifierInstance.Notifier.ShowError($"{UITexts.ExceptionThrown} ({ex.Message})");
            }
        }

        private bool ValidateInputData()
        {
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(MailAddress) && !string.IsNullOrEmpty(Password)) 
                return true;

            ToastMessenger.NotifierInstance.Notifier.ShowWarning(UITexts.NoDataRegister);
            return false;
        }
    }
}
