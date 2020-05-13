using SharedRessources.Dtos;
using System.Collections.Generic;

namespace SharedRessources.DataAccess.FileDataAccess
{
    public interface IFileDataAccess
    {
        string GetApiPathOfFile(int fileId);
        void UploadFile(SharedFile sharedFile, IList<int> AuthorizedAccessGrantedTo);
        void FileDownloaded(DownloadTransaction downloadTransaction);
        void FileDelete(int fileId);
    }
}
