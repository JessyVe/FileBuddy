using FileBuddyUI.UI.Helper;
using SharedRessources.Dtos;
using System;
using System.Collections.ObjectModel;

namespace FileBuddyUI.UI.ViewModels
{
    public class MainOverviewModel : ViewModelBase
    {
        //TODO: Remove test data
        public string Username { get; set; } = "Hello User!";
        public ObservableCollection<SharedFile> AvailableFiles { get; set; } = new ObservableCollection<SharedFile>()
        {
           new SharedFile()
           {
               OwnerUserId = "Sending Test User1",
               FileName ="readme.md",
               UploadDate = DateTime.Now
           },
           new SharedFile()
           {
               OwnerUserId = "Sending Test User1",
               FileName ="Auswertung.xls",
               UploadDate = DateTime.Now
           },

           new SharedFile()
           {
               OwnerUserId = "Sending Test User2",
               FileName ="rem.docx",
               UploadDate = DateTime.Now
           }
        };
    }
}
