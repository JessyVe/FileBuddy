using FileBuddyUI.UI.Helper;
using SharedRessources.Dtos;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FileBuddyUI.UI.ViewModels
{
    public class MainOverviewViewModel : ViewModelBase
    {
        public ObservableCollection<SharedFile> UploadFileNames { get; set; }
        public ICommand DeleteCommand { get; set; }

        public MainOverviewViewModel()
        {
            DeleteCommand = new RelayCommand(o => OnDeleteUploadFile());
            UploadFileNames = new ObservableCollection<SharedFile>();
        }

        private void OnDeleteUploadFile()
        {

        }

        public void AddUploadFiles(string[] files)
        {
            foreach (var file in files)
            {
                var sharedFile = new SharedFile()
                {
                    FileName = file
                };
                UploadFileNames.Add(sharedFile);
            }

            OnPropertyChanged(nameof(UploadFileNames));
        }
    }
}
