using System.Collections.Generic;
using SharedResources.Dtos;

namespace SharedResources.DataAccess.FileDataAccess
{
    public interface IFileDataAccess
    {
        string GetApiPathOfFile(int fileId);
        void UploadFile(SharedFile sharedFile, IList<int> authorizedAccessGrantedTo);
        void FileDownloaded(DownloadTransaction downloadTransaction);
        void FileDelete(int fileId);
    }
}
