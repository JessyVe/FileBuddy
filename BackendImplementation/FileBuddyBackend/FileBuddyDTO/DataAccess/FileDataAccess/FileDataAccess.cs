using SharedRessources.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedRessources.DataAccess.FileDataAccess
{
    /// <summary>
    /// Records file specific transactions (up- and downloads)
    /// and retrieves file paths according to given file hashes. 
    /// </summary>
    public class FileDataAccess : IFileDataAccess
    {
        private static readonly log4net.ILog Log =
         log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the file path within the API based in the given unique file hash.
        /// </summary>
        /// <param name="fileHash"></param>
        /// <returns></returns>
        public async Task<string> GetApiPathOfFile(string fileHash)
        {
            return default;
        }

        /// <summary>
        /// Saves information about successfull file shares.
        /// </summary>
        /// <param name="sharedFile"></param>
        /// <param name="AuthorizedAccessGrantedTo"></param>
        /// <returns></returns>
        public async Task UploadFile(SharedFile sharedFile, IList<string> AuthorizedAccessGrantedTo)
        {
        
        }       

        private async Task SaveFileInformation(SharedFile sharedFile)
        {
           
        }

        /// <summary>
        /// Makes to shared file visible for the user.
        /// </summary>
        /// <param name="authorizedAccessGrantedTo"></param>
        /// <param name="fileHashId"></param>
        /// <returns></returns>
        private async Task SaveAuthorizedAccessGranted(IList<string> authorizedAccessGrantedTo, string fileHashId)
        {
            
        }

        public async Task FileDownloaded(FileTransaction downloadTransaction)
        {
            // TODO: Keep a record of all download transactions.
            Log.Debug("File was downloaded. ");
        }

        /// <summary>
        /// Deletes the file identified by the given hash. 
        /// </summary>
        /// <param name="fileHash"></param>
        /// <returns></returns>
        public async Task FileDelete(string fileHash)
        {
            Log.Debug("File delete request was received. ");
        }
    }
}
