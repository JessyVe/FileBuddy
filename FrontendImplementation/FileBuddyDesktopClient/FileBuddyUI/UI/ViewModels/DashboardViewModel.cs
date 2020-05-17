using FileBuddyUI.UI.Helper;
using SharedRessources.DataAccess.ApiAccess;
using SharedRessources.DisplayedTypes;
using SharedRessources.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ToastNotifications.Messages;

namespace FileBuddyUI.UI.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public ObservableCollection<DisplayedSharedFile> ReceivedFiles { get; set; }
        public ObservableCollection<DisplayedSharedFile> SentFiles { get; set; }

        public ObservableCollection<UploadFile> ToUploadFiles { get; set; }
        public UploadFile SelectedUploadFile { get; set; }

        public DisplayedSharedFile SelectedDowloadFile { get; set; }

        public ICommand OnRemoveFileCommand { get; }
        public ICommand OnDownloadFile { get; }
        public ICommand OnUploadFiles { get; }
        public ICommand OnFetchFiles { get; }

        // TODO: Extract into class
        public string CurrentAction { get; set; }
        public Brush CurrentActionColor { get; set; }

        private readonly IList<string> _currentUploadPaths;

        public DashboardViewModel()
        {
            ReceivedFiles = new ObservableCollection<DisplayedSharedFile>();
            SentFiles = new ObservableCollection<DisplayedSharedFile>();

            ToUploadFiles = new ObservableCollection<UploadFile>();

            OnRemoveFileCommand = new RelayCommand(o => RemoveFile());
            OnDownloadFile = new RelayCommand(o => DownloadFile());
            OnUploadFiles = new RelayCommand(o => UploadFiles());
            OnFetchFiles = new RelayCommand(o => FetchFiles());

            CurrentAction = UITexts.DragFileHere;
            CurrentActionColor = (Brush)Application.Current.Resources["BuddyDarkGrey"];

            _currentUploadPaths = new List<string>();
        }

        public async void FetchFiles()
        {
            try
            {
                var fetchedFiles = await ApiClient.Instance.FetchAvailableFiles(UserInformation.Instance.CurrentUser.Id);
                var fileCount = fetchedFiles.Count;

                if(fileCount == 0)
                     ToastMessenger.NotifierInstance.ShowInformation(UITexts.NoNewFiles);
                else
                    ToastMessenger.NotifierInstance.ShowInformation(string.Format(UITexts.ReceivedFiles, fetchedFiles.Count));

                ReceivedFiles.Clear();
                foreach (var file in fetchedFiles)
                {
                    ReceivedFiles.Add(file);
                }
                CollectionSorter.Sort<DisplayedSharedFile>(ReceivedFiles);
            }
            catch (Exception ex)
            {
                ToastMessenger.NotifierInstance.ShowError($"{UITexts.ExceptionThrown} ({ex.Message})");
            }
        }

        private async void UploadFiles()
        {
            var successfullSendFiles = new List<UploadFile>();
            foreach (var uploadFile in ToUploadFiles)
            {
                try
                {
                    await ApiClient.Instance.Upload(UserInformation.Instance.CurrentUser.Id, new List<UserGroup>(), uploadFile.FullPath);
                    successfullSendFiles.Add(uploadFile);                   
                }
                catch (Exception ex)
                {
                    ToastMessenger.NotifierInstance.ShowError($"{UITexts.ExceptionThrown} ({ex.Message})");
                }
            }
            successfullSendFiles.ForEach(file => RemoveFile(file));
            ToastMessenger.NotifierInstance.ShowSuccess(string.Format(UITexts.SuccessfullUpload, successfullSendFiles.Count));
        }

        private async void DownloadFile()
        {
            try
            {
                var savedPath = await ApiClient.Instance.Download(new DownloadRequest()
                {
                    ApiPath = SelectedDowloadFile.ApiPath, 
                    ReceiverId = UserInformation.Instance.CurrentUser.Id
                });
                ToastMessenger.NotifierInstance.ShowSuccess(string.Format(UITexts.FileSavedAt, savedPath));
            }
            catch (Exception ex)
            {
                ToastMessenger.NotifierInstance.ShowError($"{UITexts.ExceptionThrown} ({ex.Message})");
            }
        }

        private void RemoveFile(UploadFile file = null)
        {
            ToUploadFiles.Remove(file ?? SelectedUploadFile);
            _currentUploadPaths.Remove(file?.FullPath ?? SelectedUploadFile?.FullPath);

            if (ToUploadFiles.Count == 0)
                SetUIToDefault();
        }

        private void SetUIToDefault()
        {
            CurrentAction = UITexts.DragFileHere;
            CurrentActionColor = (Brush)Application.Current.Resources["BuddyDarkGrey"];

            OnPropertyChanged(nameof(CurrentAction));
            OnPropertyChanged(nameof(CurrentActionColor));
        }

        public void AddUploadFile(string fullFilePath)
        {
            if (_currentUploadPaths.Contains(fullFilePath))
            {
                ToastMessenger.NotifierInstance.ShowInformation(UITexts.FileIsAlreadyShared);
                return;
            }
            if(Path.GetFileName(fullFilePath).Contains(" "))
            {
                ToastMessenger.NotifierInstance.ShowWarning(UITexts.FilenameWithBlanks);
                return;
            }

            _currentUploadPaths.Add(fullFilePath);
            var sharedFile = new UploadFile()
            {
                SharedFileName = Path.GetFileName(fullFilePath),
                FullPath = fullFilePath
            };
            ToUploadFiles.Add(sharedFile);
            CurrentAction = UITexts.ShareNow;
            CurrentActionColor = (Brush)Application.Current.Resources["BuddyGreen"];

            OnPropertyChanged(nameof(CurrentAction));
            OnPropertyChanged(nameof(CurrentActionColor));
        }
    }
}
