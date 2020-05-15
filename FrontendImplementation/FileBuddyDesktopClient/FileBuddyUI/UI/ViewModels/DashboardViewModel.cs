using FileBuddyUI.UI.Helper;
using SharedRessources.DataAccess.ApiAccess;
using SharedRessources.DisplayedTypes;
using SharedRessources.Dtos;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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

        // TODO: Extract into class
        public string CurrentAction { get; set; }
        public Brush CurrentActionColor { get; set; }

        public DashboardViewModel()
        {
            ReceivedFiles = new ObservableCollection<DisplayedSharedFile>();
            SentFiles = new ObservableCollection<DisplayedSharedFile>();

            UploadFiles = new ObservableCollection<SharedFile>();

            OnRemoveFileCommand = new RelayCommand(o => RemoveFile());
            OnDownloadFile = new RelayCommand(o => DownloadFile());

            CurrentAction = UITexts.DragFileHere;
            CurrentActionColor = (Brush)Application.Current.Resources["BuddyDarkGrey"];

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

            if (UploadFiles.Count == 0)
            {
                CurrentAction = UITexts.DragFileHere;
                CurrentActionColor = (Brush)Application.Current.Resources["BuddyDarkGrey"];
                OnPropertyChanged(nameof(CurrentAction));
                OnPropertyChanged(nameof(CurrentActionColor));
            }
        }

        public void AddUploadFile(string fullFilePath)
        {
            var sharedFile = new SharedFile()
            {
                SharedFileName = Path.GetFileName(fullFilePath),

            };
            UploadFiles.Add(sharedFile);
            CurrentAction = UITexts.ShareNow;
            CurrentActionColor = (Brush)Application.Current.Resources["BuddyGreen"];

            OnPropertyChanged(nameof(CurrentAction));
            OnPropertyChanged(nameof(CurrentActionColor));
        }
    }
}
