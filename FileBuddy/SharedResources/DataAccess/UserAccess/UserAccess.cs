using Microsoft.EntityFrameworkCore;
using SharedRessources.DisplayedTypes;
using System.Collections.Generic;
using System.Linq;
using SharedResources.Database;
using SharedResources.Dtos;

namespace SharedResources.DataAccess.UserAccess
{
    public class UserAccess : IUserAccess
    {
        private static readonly log4net.ILog Log =
         log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Fetching files on start of client.
        /// </summary>
        /// <param name="userHashId"></param>
        /// <returns></returns>
        public ICollection<DisplayedSharedFile> FetchAvailableFiles(int userId)
        {
            Log.Debug("Fetching available files. ");
            var displayedSharedFiles = new List<DisplayedSharedFile>();

            using (var context = new SQLiteDBContext())
            {
                var fileIds = context.AuthorizedAccess
                    .Where(access => access.UserId == userId)
                    .Select(access => access.SharedFileId).ToList();

                var availableFiles = context.SharedFile.Where(file => fileIds.Contains(file.Id)).ToList();
                displayedSharedFiles.AddRange(
                    from file in availableFiles 
                    let ownerName = context.AppUser.First(user => user.Id == file.OwnerUserId).Name 
                    select new DisplayedSharedFile()
                    {
                        Id = file.Id, 
                        SharedFileName = file.SharedFileName, 
                        UploadDate = file.UploadDate, 
                        OwnerName = ownerName
                    });
            }
            return displayedSharedFiles;
        }

        /// <summary>
        /// Overrides the existing user object. 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public AppUser UpdateUserInformation(AppUser user)
        {
            Log.Debug("Updating user information. ");
            using var context = new SQLiteDBContext();
            context.AppUser.Update(user);
            context.Entry(user).State = EntityState.Modified;
            context.SaveChanges();

            return context.AppUser.First(u => u.Id == user.Id);
        }

        /// <summary>
        /// Returns the user object identified by the given id. 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public AppUser GetUserInformation(int userId)
        {
            Log.Debug("Fetching user information. ");
            using var context = new SQLiteDBContext();
            return context.AppUser.FirstOrDefault(user => user.Id == userId);
        }

        /// <summary>
        /// Returns all registered user.
        /// </summary>
        /// <returns></returns>
        public IList<AppUser> LoadAllUsersFromDatabase()
        {
            Log.Debug("Fetching all users. ");
            using var context = new SQLiteDBContext();
            return context.AppUser.ToList();
        }

        /// <summary>
        /// Returns true if deletion of user was successful.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DeleteUser(int userId)
        {      
            Log.Debug("Delete user request was received. ");
            using (var context = new SQLiteDBContext())
            {
                var toDeleteUser = context.AppUser.FirstOrDefault(user => user.Id == userId);

                if (toDeleteUser == null)
                    return false;

                context.Entry(toDeleteUser).State = EntityState.Deleted;
                context.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// Deletes all entries regarding the given user. 
        /// </summary>
        /// <param name="userId"></param>
        private void DeleteAssociationsOfUser(int userId)
        {
            DeleteAuthorizations(userId);
            DeleteTransactions(userId);         
        }

        private void DeleteAuthorizations(int userId)
        {
            using var context = new SQLiteDBContext();
            var toDeleteAuthorizations = context.AuthorizedAccess.Where(access => access.UserId == userId);
            foreach (var auth in toDeleteAuthorizations)
            {
                context.Remove(context.AuthorizedAccess.Remove(auth));                   
                context.Entry(auth).State = EntityState.Deleted;
            }
            context.SaveChanges();
        }

        private void DeleteTransactions(int userId)
        {
            using var context = new SQLiteDBContext();
            var toDeleteTransactions = context.DownloadTransaction.Where(transaction => transaction.ReceiverUserId == userId);
            foreach (var transaction in toDeleteTransactions)
            {
                context.Remove(context.DownloadTransaction.Remove(transaction));
                context.Entry(transaction).State = EntityState.Deleted;
            }
            context.SaveChanges();
        }
    }
}
