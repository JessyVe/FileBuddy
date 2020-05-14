using FileBuddyUI.UI.Helper;
using SharedRessources.DataAccess.ApiAccess;
using SharedRessources.Dtos;
using SharedRessources.Services;
using System;
using System.Windows.Input;

namespace FileBuddyUI.UI.ViewModels
{
    public class LoginScreenViewModel : BaseAuthentificationViewModel
    {
        public ICommand OnLogin { get; set; }
        public ICommand OnLoginWithMac { get; set; }

        public LoginScreenViewModel()
        {
            OnLogin = new RelayCommand(o => LoginUser());
            OnLoginWithMac = new RelayCommand(o => LoginUserWithMac());
        }      

        private async void LoginUser()
        {
            try
            {
                if (string.IsNullOrEmpty(MailAddress) || string.IsNullOrEmpty(Password))
                {
                    // TODO: Show message
                }

                var user = new AppUser()
                {
                    MailAddress = MailAddress,
                    Password = Password
                };

                UIService.SetBusyState();
                var loggedInUser = await ApiClient.Instance.LoginWithMailAddress(user);
                // TODO: Check data
                OnAuthentificationSuccess(new AuthentificationEventArgs()
                {
                    AppUser = loggedInUser
                });
            } catch(Exception ex)
            {
                // TODO: Show message
            }
        }

        private async void LoginUserWithMac()
        {
            try
            {
                UIService.SetBusyState();
                var loggedInUser = await ApiClient.Instance.LoginWithMacAddress(MacAddressRetriever.GetMacAddress());
                // TODO: Check data
                OnAuthentificationSuccess(new AuthentificationEventArgs()
                {
                    AppUser = loggedInUser
                });
            }
            catch (Exception ex)
            {
                // TODO: Show message
            }
        }
    }
}
