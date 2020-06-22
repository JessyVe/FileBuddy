using System.Windows;
using System.Windows.Controls;
using FileBuddyUI.UI.ViewModels.Authentication;

namespace FileBuddyUI.UI.Views.Authentication
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
            if (Application.Current.MainWindow != null) 
                Application.Current.MainWindow.Close();
        }

        private void OnWindowMinimize(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void txPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginScreenViewModel loginScreenViewModel) 
                loginScreenViewModel.Password = txPassword.Password;
        }
    }
}
