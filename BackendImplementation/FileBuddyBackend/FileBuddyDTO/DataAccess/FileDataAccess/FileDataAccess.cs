using SharedRessources.Database;
using SharedRessources.Dtos;
using System.Collections.Generic;
using System.Linq;

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
        /// <param name="fileId"></param>
        /// <returns></returns>
        public string GetApiPathOfFile(int fileId)
        {
            Log.Debug("Api path was requested. ");
            using (var context = new SQLiteDBContext())
            {
                return context.SharedFile.First(file => file.Id == fileId)?.ApiPath ?? default;
            }
        }

        /// <summary>
        /// Saves information about successfull file shares.
        /// </summary>
        /// <param name="sharedFile"></param>
        /// <param name="AuthorizedAccessGrantedTo"></param>
        /// <returns></returns>
        public void UploadFile(SharedFile sharedFile, IList<int> AuthorizedAccessGrantedTo)
        {
            Log.Debug("File upload was completed. ");
            using (var context = new SQLiteDBContext())
            {
               var sharedFileId = context.SharedFile.Add(sharedFile).Entity.Id;
                context.SaveChanges();
            }
        }       

        private void SaveFileInformation(SharedFile sharedFile)
        {
           
        }

        /// <summary>
        /// Makes to shared file visible for the user.
        /// </summary>
        /// <param name="authorizedAccessGrantedTo"></param>
        /// <param name="fileHashId"></param>
        /// <returns></returns>
        private void SaveAuthorizedAccessGranted(IList<string> authorizedAccessGrantedTo, string fileHashId)
        {
            
        }

        public void FileDownloaded(DownloadTransaction downloadTransaction)
        {
            // TODO: Keep a record of all download transactions.
            Log.Debug("File was downloaded. ");
        }

        /// <summary>
        /// Deletes the file identified by the given hash. 
        /// </summary>
        /// <param name="fileHash"></param>
        /// <returns></returns>
        public void FileDelete(int fileId)
        {
            Log.Debug("File delete request was received. ");
        }
    }
}
