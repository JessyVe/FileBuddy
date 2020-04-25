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

        private void File_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);     
            }
            ResetView();
        }

        private void cDragArea_DragEnter(object sender, DragEventArgs e)
        {
            lbDragNDrop.Visibility = Visibility.Visible;
            tabControl.Visibility = Visibility.Hidden;
        }

        private void cDragArea_DragLeave(object sender, DragEventArgs e)
        {
            ResetView();
        }

        private void ResetView()
        {
            lbDragNDrop.Visibility = Visibility.Hidden;
            tabControl.Visibility = Visibility.Visible;
        }    

        private void OnClose(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        private void OnWindowMinimize(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void OnWindowMaximize(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }
    }
}
