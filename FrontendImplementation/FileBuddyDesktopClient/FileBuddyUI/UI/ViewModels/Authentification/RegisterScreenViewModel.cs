using FileBuddyUI.UI.Helper;
using SharedRessources.DataAccess.ApiAccess;
using SharedRessources.Dtos;
using System;
using System.Windows.Input;

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
                    // TODO: Show message
                }

                var user = new AppUser()
                {
                    Name = Username,
                    MailAddress = MailAddress,
                    Password = Password, 
                    AccountCreationDate = DateTime.Now
                };

                var loggedInUser = await ApiClient.Instance.RegisterUser(user);
                // TODO: Check data
                OnAuthentificationSuccess(new AuthentificationEventArgs()
                {
                    AppUser = loggedInUser, 
                    RegisteredAsNewUser = true
                });
            }
            catch (Exception ex)
            {
                // TODO: Show message
            }
        }
    }
}
