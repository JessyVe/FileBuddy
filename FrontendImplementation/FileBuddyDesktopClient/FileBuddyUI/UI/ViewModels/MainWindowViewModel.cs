using FileBuddyUI.UI.Helper;
using FileBuddyUI.UI.Views;
using MaterialDesignThemes.Wpf;
using SharedRessources.DataAccess.ApiAccess;
using SharedRessources.Dtos;
using System;
using System.Security;
using System.Windows.Input;

namespace FileBuddyUI.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private static readonly log4net.ILog Log =
             log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ApiClient _apiClient;

        // TODO: Extract into Model
        public string MailAddress { get; set; }
        public SecureString Password { get; set; }

        // TODO: inject down into view models via the interface
        // https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit/wiki/Snackbar
        public ISnackbarMessageQueue SnackbarMessageQueue { get; set; }

        // Commands
        public ICommand OnLoginCommand { get; set; }
        public ICommand OnRegisterCommand { get; set; }

        public MainWindowViewModel()
        {
            _apiClient = new ApiClient();
            OnLoginCommand = new RelayCommand(o => OnLogin());
            OnRegisterCommand = new RelayCommand(o => OnRegister());
        }

        private void OnLogin()
        {
            // TODO: Set as new Application.MainWindow or handle over real MainWindow
            var mainOverview = new MainOverview
            {
                DataContext = new MainOverviewModel()
            };
            mainOverview.Show();
        }

        private void OnRegister()
        {
            var newUser = new User()
            {
                MailAddress = MailAddress
            };
            try
            {
                var myUser = _apiClient.RegisterUser(newUser).Result;
                SnackbarMessageQueue.Enqueue($"Your Hash {myUser.HashId}");
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error occured while registeration. ", ex);
            }
        }
    }
}
