using SharedRessources.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedRessources.DataAccess.FileDataAccess
{
    public interface IFileDataAccess
    {
        Task<string> GetApiPathOfFile(string fileHash);
        Task UploadFile(SharedFile sharedFile, IList<string> AuthorizedAccessGrantedTo);
        Task FileDownloaded(DownloadTransaction downloadTransaction);
        Task FileDelete(string fileHash);
    }
}
