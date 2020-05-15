using FileBuddyUI.UI.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileBuddyUI.UI.Views
{
    /// <summary>
    /// Interaction logic for FileDropArea.xaml
    /// </summary>
    public partial class FileDropArea : UserControl
    {
        private readonly Brush DefaultBrush = (Brush)Application.Current.Resources["BuddyDarkGrey"];
        private readonly Brush AnimationBrush = (Brush)Application.Current.Resources["BuddyDarkOrange"];

        private readonly string DefaultDropLableText = "Drag file here";
        private readonly string AnimationDropLableText = "Release file";

        public FileDropArea()
        {
            InitializeComponent();
        }

        private void File_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                var context = DataContext as DashboardViewModel;

                foreach (var file in files)
                    context.FilePaths.Add(file);
            }
        }

        private void cDragArea_DragEnter(object sender, DragEventArgs e)
        {
            cDragArea.Background = AnimationBrush;
            lbDrop.Content = AnimationDropLableText;
        }

        private void cDragArea_DragLeave(object sender, DragEventArgs e)
        {
            cDragArea.Background = DefaultBrush;
            lbDrop.Content = DefaultDropLableText;
        }
    }
}
