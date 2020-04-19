using FileBuddyUI.UI.Helper;
using FileBuddyUI.UI.Views;
using System.Windows.Input;

namespace FileBuddyUI.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ICommand OnShowMainOverview { get; set; }

        public MainViewModel()
        {
            OnShowMainOverview = new RelayCommand(o => ShowMainOverview());
        }
         
        private void ShowMainOverview()
        {
            // TODO: Set as new Application.MainWindow or handle over real MainWindow
            var mainOverview = new MainOverview
            {
                DataContext = new MainOverviewModel()
            };
            mainOverview.Show();
        }
    }
}
