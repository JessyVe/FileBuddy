using FileBuddyUI.UI.Helper;
using SharedRessources.DisplayedTypes;
using SharedRessources.Dtos;
using System;
using System.Collections.ObjectModel;

namespace FileBuddyUI.UI.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public ObservableCollection<DisplayedSharedFile> ReceivedFiles { get; set; }
        public ObservableCollection<DisplayedSharedFile> SentFiles { get; set; }

        public DashboardViewModel()
        {
            ReceivedFiles = new ObservableCollection<DisplayedSharedFile>();
            SentFiles = new ObservableCollection<DisplayedSharedFile>();
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
        }
    }
}
