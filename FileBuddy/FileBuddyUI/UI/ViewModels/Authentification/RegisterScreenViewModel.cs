using FileBuddyUI.UI.Helper;
using SharedRessources.DataAccess.ApiAccess;
using SharedRessources.Dtos;
using System;
using System.Windows.Input;
using ToastNotifications.Messages;

namespace FileBuddyUI.UI.ViewModels
{
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
            try
            {
                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(MailAddress) || string.IsNullOrEmpty(Password))
                {
                    ToastMessenger.NotifierInstance.ShowWarning(UITexts.NoDataRegister);
                }

                var user = new AppUser()
                {
                    Name = Username,
                    MailAddress = MailAddress,
                    Password = Password,
                    AccountCreationDate = DateTime.Now
                };

                UIService.SetBusyState();
                var loggedInUser = await ApiClient.Instance.RegisterUser(user);
                if (loggedInUser.Id > 0)
                {
                    OnAuthentificationSuccess(new AuthentificationEventArgs()
                    {
                        AppUser = loggedInUser,
                        RegisteredAsNewUser = true
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
