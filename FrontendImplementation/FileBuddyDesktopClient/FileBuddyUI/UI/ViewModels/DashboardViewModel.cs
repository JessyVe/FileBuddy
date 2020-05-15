using FileBuddyUI.UI.Helper;
using SharedRessources.DisplayedTypes;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FileBuddyUI.UI.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public ObservableCollection<DisplayedSharedFile> ReceivedFiles { get; set; }
        public ObservableCollection<DisplayedSharedFile> SentFiles { get; set; }

        public ObservableCollection<string> FilePaths { get; set; }
        public string SelectedFile { get; set; }

        private ICommand OnRemoveFileCommand;

        public DashboardViewModel()
        {
            ReceivedFiles = new ObservableCollection<DisplayedSharedFile>();
            SentFiles = new ObservableCollection<DisplayedSharedFile>();

            FilePaths = new ObservableCollection<string>();

            OnRemoveFileCommand = new RelayCommand(o => RemoveFile());

            GenerateDemoData();
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

            FilePaths.Add("asdf1");
            FilePaths.Add("asdf2");
            FilePaths.Add("asdf3jshdfashidflakshdfklasdhfkaujdflö");
        }

        private void FileDropped(string[] filePaths)
        {
            foreach (var filePath in filePaths)
                FilePaths.Add(filePath);
        }

        private void RemoveFile()
        {
            
        }
    }
}
