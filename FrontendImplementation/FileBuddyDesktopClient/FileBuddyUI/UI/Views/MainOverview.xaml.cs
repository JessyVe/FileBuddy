using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FileBuddyUI.UI.Views
{
    /// <summary>
    /// Interaction logic for MainOverview.xaml
    /// </summary>
    public partial class MainOverview : Window
    {
        public MainOverview()
        {
            InitializeComponent();
        }
        private void OnClose(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }
    }
}
