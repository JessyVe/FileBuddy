using FileBuddyUI.Helper;
using FileBuddyUI.UI.Helper;
using FileBuddyUI.UI.ViewModels;
using System.Windows;
using System.Windows.Input;
using ToastNotifications.Messages;

namespace FileBuddyUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly LoginScreenViewModel _loginScreenViewModel;
        private readonly RegisterScreenViewModel _registerScreenViewModel;
        private readonly DashboardViewModel _dashboardViewModel;

        public MainWindow()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;

            _loginScreenViewModel = new LoginScreenViewModel();
            _registerScreenViewModel = new RegisterScreenViewModel();
            _dashboardViewModel = new DashboardViewModel();          

            _loginScreenViewModel.AuthentificationSuccess += OnAuthentificationenSuccess;
            _registerScreenViewModel.AuthentificationSuccess += OnAuthentificationenSuccess;

            DataContext = _loginScreenViewModel; 
        }

        private void OnAuthentificationenSuccess(object sender, System.EventArgs e)
        {
            var args = e as AuthentificationEventArgs;
            UserInformation.Instance.CurrentUser = args.AppUser;

            if(args.RegisteredAsNewUser)
                ToastMessenger.NotifierInstance.ShowSuccess(UITexts.WelcomeText);
            else
                ToastMessenger.NotifierInstance.ShowSuccess(string.Format(UITexts.WelcomeBack, args.AppUser.Name));

            DataContext = _dashboardViewModel;
            _dashboardViewModel.FetchFiles();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is LoginScreenViewModel)
            {
                btBackToLogin.Visibility = Visibility.Hidden;
                lbRegister.Visibility = Visibility.Visible;
            }
            else if (DataContext is RegisterScreenViewModel)
            {
                btBackToLogin.Visibility = Visibility.Visible;
                lbRegister.Visibility = Visibility.Hidden;
            } else if (DataContext is DashboardViewModel)
            {
                btBackToLogin.Visibility = Visibility.Hidden;
                lbRegister.Visibility = Visibility.Collapsed;
            }
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OnWindowMinimize(object sender, RoutedEventArgs e)
        {
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

        private void OnLoginLableMouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void OnLoginLableMouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }
    }
}
