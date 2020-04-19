using System;
using System.Net;

namespace SharedRessources.DataAccess
{
    public class FtpAccess : IFileAccess
    {
        // TODO: Transfer to user settings; Make editable so user can use their on FTP
        private const string FTPServerUrl = "ftp://www.jessicaveit.bplaced.net";
        private const string UserName = "jessicaveit";
        private const string Password = "fhjoanneum220";

        /// <summary>
        /// Uploads the given file to a given destination path 
        /// on the ftp server.
        /// </summary>
        /// <param name="localFilePath"></param>
        /// <param name="uploadTargetPath"></param>
        public bool UploadFile(string localFilePath, string uploadTargetPath)
        {
            //TODO: Implement asynchronous; 
            // https://stackoverflow.com/questions/1542853/webclient-upload-file-error
            // https://code5.cn/so/c%23/1676970

            try
            {
                using (var client = new WebClient())
                {
                    client.Credentials = new NetworkCredential(UserName, Password);
                    client.BaseAddress = FTPServerUrl;
                    client.UploadFile(address: uploadTargetPath,
                                      fileName: localFilePath);
                }
            }
            catch (WebException ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Dowloads the extrenal file from the server
        /// and saves it on the defined path.
        /// </summary>
        /// <param name="remoteFilePath"></param>
        /// <param name="downloadTargetPath"></param>
        public bool DownloadFile(string remoteFilePath, string downloadTargetPath)
        {
            try
            {
                using (var client = new WebClient())
                {
                    // TODO: Dont override File!
                    client.Credentials = new NetworkCredential(UserName, Password);
                    client.BaseAddress = FTPServerUrl;
                    client.DownloadFile(address: remoteFilePath,
                                        fileName: downloadTargetPath);
                }
            }
            catch (WebException ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Deletes a file from the server. 
        /// </summary>
        /// <param name="remoteFilePath"></param>
        public bool DeleteFile(string remoteFilePath)
        {
            try
            {
                var completeRemotePath = FTPServerUrl + remoteFilePath.Replace("\\", "/");
                var deleteRequest = (FtpWebRequest)WebRequest.Create(completeRemotePath);

                deleteRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                deleteRequest.Credentials = new NetworkCredential(UserName, Password);

                using (var response = (FtpWebResponse)deleteRequest.GetResponse())
                {
                    //TODO: Log information; Verify Status if was successfull
                    var responseStatusDescription = response.StatusDescription;
                }
            }
            catch (UriFormatException ex)
            {
                return false;
            }
            return true;
        }
    }
}
