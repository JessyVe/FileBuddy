using FileBuddyUI.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace FileBuddyUI.UI.Views
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
            Application.Current.MainWindow.Close();
        }

        private void OnWindowMinimize(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void txPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var registerScreenViewModel = DataContext as RegisterScreenViewModel;
            registerScreenViewModel.Password = txPassword.Password;
        }
    }
}
