using FileBuddyUI.UI.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace FileBuddyUI.UI.Views
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : UserControl
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void FileRepresentation_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var context = DataContext as DashboardViewModel;
            context.OnDownloadFile.Execute(null);
        }
    }
}
