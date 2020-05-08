using Firebase.Database.Query;
using SharedRessources.Dtos;
using System.Threading.Tasks;

namespace SharedRessources.DataAccess.FileDataAccess
{
    /// <summary>
    /// Records file specific transactions (up- and downloads)
    /// and retrieves file paths according to given file hashes. 
    /// </summary>
    public class FileDataAccess : FireBaseAccessBase, IFileDataAccess
    {
        /// <summary>
        /// Returns the file path within the API based in the given unique file hash.
        /// </summary>
        /// <param name="fileHash"></param>
        /// <returns></returns>
        public async Task<string> GetApiPathOfFile(string fileHash)
        {
            Log.Debug("File path was requested. ");                   
            var response = await _firebaseClient.Child($"files/{fileHash}").OnceAsync<SharedFile>(); 
            var sharedFile = RetrieveFirstOrDefault(response);

            return sharedFile?.APIPath ?? string.Empty;
        }

        /// <summary>
        /// Saves information about successfull file uploads to the API.
        /// </summary>
        /// <param name="apiPath"></param>
        /// <param name="ownerId"></param>
        /// <param name="receiver"></param>
        public async Task UploadFile(SharedFile sharedFile)
        {
            Log.Debug("File was uploaded to API. ");
            var response = await _firebaseClient.Child($"files/{sharedFile.HashId}").PostAsync(sharedFile);

            if (response != null)
                Log.Debug("Information was saved successfully!");
            else
                Log.Debug("Response of save action was null. ");
        }

        public async Task FileDownloaded(string downloaderId, string fileHash)
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
            await _firebaseClient.Child($"files/{fileHash}").DeleteAsync();
        }
    }
}
