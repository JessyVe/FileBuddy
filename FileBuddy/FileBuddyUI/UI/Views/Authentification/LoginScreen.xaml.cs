using FileBuddyUI.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace FileBuddyUI.UI.Views
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : UserControl
    {
        public LoginScreen()
        {
            InitializeComponent();
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        private void OnWindowMinimize(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void txPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var loginScreenViewModel = DataContext as LoginScreenViewModel;
            loginScreenViewModel.Password = txPassword.Password;
        }
    }
}
