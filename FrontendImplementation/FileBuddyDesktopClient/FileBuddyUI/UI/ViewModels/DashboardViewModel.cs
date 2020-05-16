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

        private IList<string> CurrentUploadPaths;

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

            CurrentUploadPaths = new List<string>();
        }

        private async void FetchFiles()
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
        }

        private async void DownloadFile()
        {
            try
            {
                var result = await ApiClient.Instance.Download(SelectedDowloadFile.ApiPath);
            }
            catch (Exception ex)
            {
                ToastMessenger.NotifierInstance.ShowError($"{UITexts.ExceptionThrown} ({ex.Message})");
            }
        }

        private void RemoveFile(UploadFile file = null)
        {
            ToUploadFiles.Remove(file ?? SelectedUploadFile);
            CurrentUploadPaths.Remove(file?.FullPath ?? SelectedUploadFile?.FullPath);

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
            if (CurrentUploadPaths.Contains(fullFilePath))
            {
                ToastMessenger.NotifierInstance.ShowInformation(UITexts.FileIsAlreadyShared);
                return;
            }

            CurrentUploadPaths.Add(fullFilePath);
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
