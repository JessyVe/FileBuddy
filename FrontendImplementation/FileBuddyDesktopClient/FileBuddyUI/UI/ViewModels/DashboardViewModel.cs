using FileBuddyUI.UI.Helper;
using SharedRessources.DataAccess.ApiAccess;
using SharedRessources.DisplayedTypes;
using SharedRessources.Dtos;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace FileBuddyUI.UI.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public ObservableCollection<DisplayedSharedFile> ReceivedFiles { get; set; }
        public ObservableCollection<DisplayedSharedFile> SentFiles { get; set; }

        public ObservableCollection<SharedFile> UploadFiles { get; set; }
        public SharedFile SelectedUploadFile { get; set; }

        public DisplayedSharedFile SelectedDowloadFile { get; set; }

        public ICommand OnRemoveFileCommand { get; }
        public ICommand OnDownloadFile { get; }

        public DashboardViewModel()
        {
            ReceivedFiles = new ObservableCollection<DisplayedSharedFile>();
            SentFiles = new ObservableCollection<DisplayedSharedFile>();

            UploadFiles = new ObservableCollection<SharedFile>();

            OnRemoveFileCommand = new RelayCommand(o => RemoveFile());
            OnDownloadFile = new RelayCommand(o => DownloadFile());

            GenerateDemoData();
        }

        private async void DownloadFile()
        {
            try
            {
                var result = await ApiClient.Instance.Download(SelectedDowloadFile.ApiPath);
            }
            catch (Exception ex)
            {
                // TODO: Show message
            }
        }

        private void GenerateDemoData()
        {
            ReceivedFiles.Add(new DisplayedSharedFile()
            {
                SharedFileName = "Received.txt", 
                UploadDate = DateTime.Now, 
                OwnerName = "M1ke"
            });

            SentFiles.Add(new DisplayedSharedFile()
            {
                SharedFileName = "Sent.txt",
                UploadDate = DateTime.Now,
                OwnerName = "Tony"
            });
        }

        private void RemoveFile()
        {
            UploadFiles.Remove(SelectedUploadFile);
        }

        public void AddUploadFile(string fullFilePath)
        {
            var sharedFile = new SharedFile()
            {
                SharedFileName = Path.GetFileName(fullFilePath),

            };
            UploadFiles.Add(sharedFile);
        }
    }
}
