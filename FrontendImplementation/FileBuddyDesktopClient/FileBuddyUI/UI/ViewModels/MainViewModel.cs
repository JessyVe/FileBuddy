using FileBuddyUI.UI.Views;

namespace FileBuddyUI.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        private void ShowMainOverview()
        {
            var sw = new MainOverview();
            sw.Show();
        }
    }
}
