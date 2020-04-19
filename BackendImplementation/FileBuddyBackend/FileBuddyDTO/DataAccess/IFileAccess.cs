namespace SharedRessources.DataAccess
{
    public interface IFileAccess
    {
        public bool UploadFile(string localFilePath, string uploadTargetPath);
        public bool DownloadFile(string remoteFilePath, string downloadTargetPath);

        public bool DeleteFile(string remoteFilePath);
    }
}
