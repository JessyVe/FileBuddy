using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedRessources.DataAccess;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BackendTests.FileAccessTests
{
    [TestClass]
    public class FtpAccessTests
    {
        private readonly IFileAccess _fileAccess;
        private readonly string _resourcePath;
        private readonly string _tmpPath;

        public FtpAccessTests()
        {
            _fileAccess = new FtpAccess();
            _resourcePath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "resources";

            _tmpPath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "tmp";
            Directory.CreateDirectory(_tmpPath);
        }

        [TestInitialize]
        public void ClearTmpDirectory()
        {
            var di = new DirectoryInfo(_tmpPath);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Directory.Delete(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "tmp");
        }

        [TestMethod]
        public void DownloadFile()
        {
            // try upload files
            var remotelocalFilePaths = new List<string>();
            foreach (var localFilePath in Directory.GetFiles(_resourcePath))
            {
                var remoteFilePath = "\\test\\" + Path.GetFileName(localFilePath);
                var success = DoFileUpload(localFilePath, remoteFilePath);
                remotelocalFilePaths.Add(remoteFilePath);
            }

            // try dowload files
            foreach (var remoteFilePath in remotelocalFilePaths)
            {
                var tmplocalFilePath = _tmpPath + "\\" + Path.GetFileName(remoteFilePath);
                _fileAccess.DownloadFile(remoteFilePath, tmplocalFilePath);
                Assert.IsTrue(Directory.GetFiles(_tmpPath).Any(tmplocalFilePath.Contains));
            }

            // TODO: Implement teardown -> delete files from ftp
        }

        [TestMethod]
        public void UploadFile()
        {
            foreach (var localFilePath in Directory.GetFiles(_resourcePath))
            {
                var remoteFilePath = "\\test\\" + Path.GetFileName(localFilePath);
                var success = DoFileUpload(localFilePath, remoteFilePath);

                Assert.IsTrue(success);
            }

        }

        [TestMethod]
        public void DeleteRemoteFile()
        {
            foreach (var localFilePath in Directory.GetFiles(_resourcePath))
            {
                var remoteFilePath = "\\test\\" + Path.GetFileName(localFilePath);

                DoFileUpload(localFilePath, remoteFilePath);

                var success = _fileAccess.DeleteFile(remoteFilePath);
                Assert.IsTrue(success);
            }
        }

        private bool DoFileUpload(string localFilePath, string remoteFilePath)
        {
            return _fileAccess.UploadFile(localFilePath, remoteFilePath);
        }
    }
}
