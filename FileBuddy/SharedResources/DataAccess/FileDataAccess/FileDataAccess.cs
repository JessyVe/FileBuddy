using Microsoft.EntityFrameworkCore;
using SharedRessources.Database;
using SharedRessources.Dtos;
using System;
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
            Log.Debug("File path was requested. ");
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
            Log.Debug("Information about new file upload will be save in database.");
            using (var context = new SQLiteDBContext())
            {
                context.SharedFile.Add(sharedFile);
                context.Entry(sharedFile).State = EntityState.Added;
                context.SaveChanges();
            }

            if (sharedFile.Id == 0)
            {
                Log.Error("Unable to save file information in database.");
                throw new Exception("Database insert faild.");
            }
            SaveAuthorizedAccessGranted(authorizedAccessGrantedTo, sharedFile.Id);
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
                    var authorizedAccess = new AuthorizedAccess()
                    {
                        SharedFileId = sharedFileId,
                        UserId = userId
                    };
                    context.AuthorizedAccess.Add(authorizedAccess);
                    context.Entry(authorizedAccess).State = EntityState.Added;
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
                context.Entry(downloadTransaction).State = EntityState.Added;
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

            var authorizedAccesses = new List<AuthorizedAccess>();
            var sharedFiles = new List<SharedFile>();
            using (var context = new SQLiteDBContext())
            {
                authorizedAccesses = context.AuthorizedAccess.Where(access => access.SharedFileId == fileId).ToList();
                sharedFiles = context.SharedFile.Where(sharedFile => sharedFile.Id == fileId).ToList();
            }
            RemoveAuthorizations(authorizedAccesses);
            RemoveFiles(sharedFiles);
        }

        private void RemoveAuthorizations(IList<AuthorizedAccess> authorizedAccesses)
        {
            using (var context = new SQLiteDBContext())
            {
                foreach (var access in authorizedAccesses)
                {
                    context.AuthorizedAccess.Remove(access);
                    context.Entry(access).State = EntityState.Deleted;
                }
                context.SaveChanges();
            }
        }

        private void RemoveFiles(IList<SharedFile> sharedFiles)
        {
            using (var context = new SQLiteDBContext())
            {
                foreach (var sharedFile in sharedFiles)
                {
                    context.SharedFile.Remove(sharedFile);
                    context.Entry(sharedFile).State = EntityState.Deleted;
                }
                context.SaveChanges();
            }
        }
    }
}
