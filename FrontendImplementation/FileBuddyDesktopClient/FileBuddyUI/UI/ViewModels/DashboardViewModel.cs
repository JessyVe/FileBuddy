using FileBuddyUI.UI.Helper;
using SharedRessources.Dtos;
using System;
using System.Collections.ObjectModel;

namespace FileBuddyUI.UI.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public ObservableCollection<SharedFile> ReceivedFiles { get; set; }
        public ObservableCollection<SharedFile> SentFiles { get; set; }

        public DashboardViewModel()
        {
            ReceivedFiles = new ObservableCollection<SharedFile>();
            SentFiles = new ObservableCollection<SharedFile>();
            GenerateDemoData();
        }

        private void GenerateDemoData()
        {
            ReceivedFiles.Add(new SharedFile()
            {
                SharedFileName = "Received.txt", 
                UploadDate = DateTime.Now
            });

            SentFiles.Add(new SharedFile()
            {
                SharedFileName = "Sent.txt",
                UploadDate = DateTime.Now
            });
        }
    }
}
