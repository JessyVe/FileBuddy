using SharedRessources.Dtos;
using System.Threading.Tasks;

namespace SharedRessources.DataAccess.FileDataAccess
{
    public interface IFileDataAccess
    {
        Task<string> GetApiPathOfFile(string fileHash);
        Task UploadFile(SharedFile sharedFile);
        Task FileDownloaded(string downloaderId, string fileHash);
        Task FileDelete(string fileHash);
    }
}
