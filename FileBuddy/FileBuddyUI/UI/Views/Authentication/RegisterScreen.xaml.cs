using System.Windows;
using System.Windows.Controls;
using FileBuddyUI.UI.ViewModels.Authentication;

namespace FileBuddyUI.UI.Views.Authentication
{
    /// <summary>
    /// Interaction logic for RegisterScreen.xaml
    /// </summary>
    public partial class RegisterScreen : UserControl
    {
        public RegisterScreen()
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
            if (DataContext is RegisterScreenViewModel registerScreenViewModel) 
                registerScreenViewModel.Password = txPassword.Password;
        }
    }
}
