using FileBuddyUI.Helper;
using FileBuddyUI.UI.Helper;
using SharedRessources.DataAccess.ApiAccess;
using SharedRessources.DisplayedTypes;
using SharedRessources.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ToastNotifications.Messages;

namespace FileBuddyUI.UI.ViewModels
{
    /// <summary>
    /// Logic for the dashboard UI.
    /// </summary>
    public class DashboardViewModel : PropertyChangedBase
    {
        private static readonly log4net.ILog Log =
                 log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ObservableCollection<DisplayedSharedFile> ReceivedFiles { get; set; }
        public ObservableCollection<DisplayedSharedFile> SentFiles { get; set; }
        public ObservableCollection<UploadFile> ToUploadFiles { get; set; }

        public UploadFile SelectedUploadFile { get; set; }
        public DisplayedSharedFile SelectedDowloadFile { get; set; }

        public ICommand OnRemoveFileCommand { get; }
        public ICommand OnDownloadFile { get; }
        public ICommand OnUploadFiles { get; }
        public ICommand OnFetchFiles { get; }


        public string CurrentDragAreaAction { get; set; }
        public Brush CurrentDragAreaActionColor { get; set; }


        private readonly IList<string> _currentUploadPaths;

        public DashboardViewModel()
        {
            // Initialize collections
            ReceivedFiles = new ObservableCollection<DisplayedSharedFile>();
            SentFiles = new ObservableCollection<DisplayedSharedFile>();
            ToUploadFiles = new ObservableCollection<UploadFile>();
            _currentUploadPaths = new List<string>();

            // Initialize ui events
            OnRemoveFileCommand = new RelayCommand(o => RemoveFile());
            OnDownloadFile = new RelayCommand(o => DownloadFile());
            OnUploadFiles = new RelayCommand(o => UploadFiles());
            OnFetchFiles = new RelayCommand(o => FetchFiles());

            CurrentDragAreaAction = UITexts.DragFileHere;
            CurrentDragAreaActionColor = (Brush)Application.Current.Resources["BuddyDarkGrey"];

            // Subscribe to client socket event (triggered if data is received)
            FileBuddyClient.Instance.NewUpdateRequestReceived += new EventHandler(delegate (Object o, EventArgs a)
            {
                Task.Run(() =>
                App.Current.Dispatcher.Invoke(async delegate
                {
                    await FetchFiles();
                }));
            });
        }

        /// <summary>
        /// Fetches file information for files, which are downloadabel 
        /// for the signed in user.
        /// </summary>
        /// <returns></returns>
        public async Task FetchFiles()
        {
            Log.Debug("Attempting to fetch files from API.");
            try
            {
                var fetchedFiles = await ApiClient.Instance.FetchAvailableFiles(UserInformation.Instance.CurrentUser.Id, UserInformation.Instance.CurrentUser.AccessToken);

                if (fetchedFiles.Count == 0)
                    ToastMessenger.NotifierInstance.ShowInformation(UITexts.NoNewFiles);
                else
                    ToastMessenger.NotifierInstance.ShowInformation(string.Format(UITexts.ReceivedFiles, fetchedFiles.Count));

                int newlyFetchedFiles = 0;
                foreach (var fetchedFile in fetchedFiles)
                {
                    if (!IsFileAlreadyFetched(fetchedFile))
                    {
                        newlyFetchedFiles++;
                        ReceivedFiles.Add(fetchedFile);
                    }
                }
                Log.Debug($"Received {newlyFetchedFiles} new files(s)! ");
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("An error occured while fetching files from API. ", ex);
                ToastMessenger.NotifierInstance.ShowError($"{UITexts.ExceptionThrown} ({ex.Message})");
            }
        }

        private bool IsFileAlreadyFetched(DisplayedSharedFile displayedSharedFile)
        {
            foreach (var alreadyFetchedFile in ReceivedFiles)
            {
                if (displayedSharedFile.ApiPath == alreadyFetchedFile.ApiPath)
                {                    
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Uploads files to the API and sends notification via web socket.
        /// </summary>
        private async void UploadFiles()
        {
            if (ToUploadFiles.Count == 0)
                return;

            Log.Debug("Attempting to upload files to API.");

            var successfullSendFiles = new List<UploadFile>();
            foreach (var uploadFile in ToUploadFiles)
            {
                try
                {
                    await ApiClient.Instance.Upload(UserInformation.Instance.CurrentUser.Id, new List<UserGroup>(), uploadFile.FullPath, UserInformation.Instance.CurrentUser.AccessToken);
                    successfullSendFiles.Add(uploadFile);

                    if (!FileBuddyClient.Instance.IsConnected)
                    {
                        Log.Error("An error occured while uploading files. Update request to web server could not be send. Not connected to server. ");
                        ToastMessenger.NotifierInstance.ShowError("You are not connected to a server. Realtime update may not be possible!");
                    }
                    else
                    {
                        await FileBuddyClient.Instance.Send(UserInformation.Instance.CurrentUser.Id);
                        Log.Debug("Update request was sent to server. ");
                    }
                }
                catch (Exception ex)
                {
                    ToastMessenger.NotifierInstance.ShowError($"{UITexts.ExceptionThrown} ({ex.Message})");
                }
            }
            Log.Debug($"{ToUploadFiles.Count} file(s) were uploaded to the API. ");

            if (successfullSendFiles.Count > 0)
            {
                successfullSendFiles.ForEach(file => RemoveFile(file));
                ToastMessenger.NotifierInstance.ShowSuccess(string.Format(UITexts.SuccessfullUpload, successfullSendFiles.Count));
            }
        }

        /// <summary>
        /// Downloads request file form API.
        /// </summary>
        private async void DownloadFile()
        {
            Log.Debug("Attempting to download files from API.");
            try
            {
                var savedPath = await ApiClient.Instance.Download(new DownloadRequest()
                {
                    ApiPath = SelectedDowloadFile.ApiPath,
                    ReceiverId = UserInformation.Instance.CurrentUser.Id
                }, UserInformation.Instance.CurrentUser.AccessToken);

                Log.Debug($"File was downloaded and save at: {savedPath}");
                ToastMessenger.NotifierInstance.ShowSuccess(string.Format(UITexts.FileSavedAt, savedPath));
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("An error occured while downloading files from API. ", ex);
                ToastMessenger.NotifierInstance.ShowError($"{UITexts.ExceptionThrown} ({ex.Message})");
            }
        }

        /// <summary>
        /// Removes either the given file or the object 
        /// containing the selected path from the upload list.
        /// </summary>
        /// <param name="file"></param>
        private void RemoveFile(UploadFile file = null)
        {
            ToUploadFiles.Remove(file ?? SelectedUploadFile);
            _currentUploadPaths.Remove(file?.FullPath ?? SelectedUploadFile?.FullPath);

            if (ToUploadFiles.Count == 0)
                SetUIToDefault();
        }

        /// <summary>
        /// Resets the UI to its initial state.
        /// </summary>
        private void SetUIToDefault()
        {
            CurrentDragAreaAction = UITexts.DragFileHere;
            CurrentDragAreaActionColor = (Brush)Application.Current.Resources["BuddyDarkGrey"];

            OnPropertyChanged(nameof(CurrentDragAreaAction));
            OnPropertyChanged(nameof(CurrentDragAreaActionColor));
        }

        /// <summary>
        /// Adds a new object to the upload list and changes the ui accordingly. 
        /// </summary>
        /// <param name="fullFilePath"></param>
        public void AddUploadFile(string fullFilePath)
        {
            if (_currentUploadPaths.Contains(fullFilePath))
            {
                ToastMessenger.NotifierInstance.ShowInformation(UITexts.FileIsAlreadyShared);
                return;
            }
            // filenames containing blanks can not be processed by the API
            if (Path.GetFileName(fullFilePath).Contains(" "))
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

            CurrentDragAreaAction = UITexts.ShareNow;
            CurrentDragAreaActionColor = (Brush)Application.Current.Resources["BuddyGreen"];

            OnPropertyChanged(nameof(CurrentDragAreaAction));
            OnPropertyChanged(nameof(CurrentDragAreaActionColor));
        }
    }
}
