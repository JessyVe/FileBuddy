using FileBuddyUI.UI.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace FileBuddyUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly LoginScreenViewModel _loginScreenViewModel;
        private readonly RegisterScreenViewModel _registerScreenViewModel;

        public MainWindow()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;

            _loginScreenViewModel = new LoginScreenViewModel();
            _registerScreenViewModel = new RegisterScreenViewModel();

            DataContext = _loginScreenViewModel;
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

        private void OnLoginLableMouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void OnLoginLableMouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void OnSwitchToRegisterScreen(object sender, MouseButtonEventArgs e)
        {
            DataContext = _registerScreenViewModel;
        }
    }
}
