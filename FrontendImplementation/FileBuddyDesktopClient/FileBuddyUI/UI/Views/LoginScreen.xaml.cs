using SharedRessources.DataAccess.ApiAccess;
using SharedRessources.Dtos;
using SharedRessources.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
    }
}
