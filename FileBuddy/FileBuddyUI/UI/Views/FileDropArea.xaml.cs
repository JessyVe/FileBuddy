using FileBuddyUI.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FileBuddyUI.UI.Views
{
    /// <summary>
    /// Interaction logic for FileDropArea.xaml
    /// </summary>
    public partial class FileDropArea : UserControl
    {
        private readonly Brush AnimationBrush = (Brush)Application.Current.Resources["BuddyDarkOrange"];

        private string _lastState;
        private Brush _lastBrush;

        public FileDropArea()
        {
            InitializeComponent();
        }

        private void File_Drop(object sender, DragEventArgs e)
        {
            ReturnToDefault();

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                var context = DataContext as DashboardViewModel;

                foreach (var file in files)
                    context.AddUploadFile(file);
            }
        }

        private void cDragArea_DragEnter(object sender, DragEventArgs e)
        {
            _lastState = lbDrop.Content.ToString();
            _lastBrush = cDragArea.Background;

            lbDrop.Content = UITexts.ReleaseFile;
            cDragArea.Background = AnimationBrush;
        }

        private void cDragArea_DragLeave(object sender, DragEventArgs e)
        {
            ReturnToDefault();
        }

        private void ReturnToDefault()
        {
            cDragArea.Background = _lastBrush;
            lbDrop.Content = _lastState;
        }
    }
}
