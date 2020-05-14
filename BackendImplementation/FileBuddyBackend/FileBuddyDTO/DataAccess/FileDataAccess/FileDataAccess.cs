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
        public void UploadFile(SharedFile sharedFile, IList<int> authorizedAccessGrantedTo)
        {
            Log.Debug("File upload was completed. ");
            var sharedFileId = -1;
            using (var context = new SQLiteDBContext())
            {
                sharedFileId = context.SharedFile.Add(sharedFile).Entity.Id;
                context.SaveChanges();
            }
            SaveAuthorizedAccessGranted(authorizedAccessGrantedTo, sharedFileId);
        }

        /// <summary>
        /// Makes to shared file visible for the user.
        /// </summary>
        /// <param name="authorizedAccessGrantedTo"></param>
        /// <param name="sharedFileId"></param>
        /// <returns></returns>
        private void SaveAuthorizedAccessGranted(IList<int> authorizedAccessGrantedTo, int sharedFileId)
        {
            using (var context = new SQLiteDBContext())
            {
                foreach (var userId in authorizedAccessGrantedTo)
                {
                    context.AuthorizedAccess.Add(new AuthorizedAccess()
                    {
                        SharedFileId = sharedFileId,
                        UserId = userId
                    });
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Saves a download transaction to the database to 
        /// enable sufficient tracking of file movements. 
        /// </summary>
        /// <param name="downloadTransaction"></param>
        public void FileDownloaded(DownloadTransaction downloadTransaction)
        {
            Log.Debug("File was downloaded. ");
            using (var context = new SQLiteDBContext())
            {
                context.DownloadTransaction.Add(downloadTransaction);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes the file identified by the given hash. 
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public void FileDelete(int fileId)
        {
            Log.Debug("File delete request was received. ");
            using (var context = new SQLiteDBContext())
            {
                context.AuthorizedAccess
                    .RemoveRange(context.AuthorizedAccess.Where(access => access.SharedFileId == fileId));
                context.SharedFile
                    .RemoveRange(context.SharedFile.Where(sharedFile => sharedFile.Id == fileId));

                context.SaveChanges();
            }
        }
    }
}
