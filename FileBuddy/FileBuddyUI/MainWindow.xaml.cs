using FileBuddyUI.Helper;
using FileBuddyUI.UI.Helper;
using FileBuddyUI.UI.ViewModels;
using System.Windows;
using System.Windows.Input;
using ToastNotifications.Messages;
using System.Threading.Tasks;
using System.Reflection;
using SharedRessources.Services;
using FileBuddyUI.UI.Helper.CustomEventArgs;
using FileBuddyUI.Resources;
using FileBuddyUI.UI.ViewModels.Authentication;

namespace FileBuddyUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly log4net.ILog Log =
              log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly LoginScreenViewModel _loginScreenViewModel;
        private readonly RegisterScreenViewModel _registerScreenViewModel;
        private readonly DashboardViewModel _dashboardViewModel;

        public MainWindow()
        {
            InitializeComponent();
            LoggingConfigurationLoader.LoadLoggingConfiguration(Assembly.GetEntryAssembly());
            Log.Info("*** Welcome to FileBuddy! ***");
            Log.Debug("UI is being set up...");

            DataContextChanged += OnDataContextChanged;

            _loginScreenViewModel = new LoginScreenViewModel();
            _registerScreenViewModel = new RegisterScreenViewModel();
            _dashboardViewModel = new DashboardViewModel();

            _loginScreenViewModel.AuthentificationSuccess += OnAuthenticationSuccess;
            _registerScreenViewModel.AuthentificationSuccess += OnAuthenticationSuccess;

            DataContext = _loginScreenViewModel;

            Log.Debug("Initialization of components finished!");
        }

        /// <summary>
        /// Handles the AuthenticationSuccess event fired by either 
        /// the login or the registration view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnAuthenticationSuccess(object sender, System.EventArgs e)
        {
            Log.Debug("User authentication succeeded.");

            var args = e as AuthenticationEventArgs;
            UserInformation.Instance.CurrentUser = args.AppUser;

            ToastMessenger.NotifierInstance.Notifier.ShowSuccess(args.IsNewUser
                ? UITexts.WelcomeText
                : string.Format(UITexts.WelcomeBack, args.AppUser.Name));

            DataContext = _dashboardViewModel;

            Log.Debug("Files will be fetched from API...");
            await _dashboardViewModel.FetchFiles();

            Log.Debug("Connection to socket server is established...");
            await ConnectToSocketServer();
        }

        /// <summary>
        /// Connects the application to the configured web server.
        /// </summary>
        /// <returns></returns>
        private async Task ConnectToSocketServer()
        {
            await FileBuddyClient.Instance.ConnectToServer().ContinueWith(_ =>
            {
                if (FileBuddyClient.Instance.IsConnected)
                    Log.Info($"Successfully connected to socket server ({SettingsHelper.Instance.ApplicationSettings.SocketServerAddress}:{SettingsHelper.Instance.ApplicationSettings.SocketServerPort})");
                else
                    Log.Error($"Unable to connect to socket server ({SettingsHelper.Instance.ApplicationSettings.SocketServerAddress}:{SettingsHelper.Instance.ApplicationSettings.SocketServerPort})");
            });
        }

        /// <summary>
        /// Changes the UI appearance based on the current context.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Log.Debug("Data context of main window was changed. UI will be updated.");
            switch (DataContext)
            {
                case LoginScreenViewModel _:
                    btBackToLogin.Visibility = Visibility.Hidden;
                    lbRegister.Visibility = Visibility.Visible;
                    break;
                case RegisterScreenViewModel _:
                    btBackToLogin.Visibility = Visibility.Visible;
                    lbRegister.Visibility = Visibility.Hidden;
                    break;
                case DashboardViewModel _:
                    btBackToLogin.Visibility = Visibility.Hidden;
                    lbRegister.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            Log.Info("Application is closing!");
            Application.Current.Shutdown();
        }

        private void OnWindowMinimize(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void OnSwitchToLoginScreen(object sender, RoutedEventArgs e)
        {
            DataContext = _loginScreenViewModel;
        }

        private void OnSwitchToRegisterScreen(object sender, MouseButtonEventArgs e)
        {
            DataContext = _registerScreenViewModel;
        }

        private void OnLoginLabelMouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void OnLoginLabelMouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void WindowMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
