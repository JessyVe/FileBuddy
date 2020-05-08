using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedRessources.DataAccess.FileDataAccess;
using SharedRessources.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendTests.DataAccessTests
{
    [TestClass]
    public class FileDataAccessTests
    {
        private readonly IFileDataAccess _fileDataAccess;
        private SharedFile _sharedFile;

        private const string FileHash = "HashId1";
        private const string APIPath = "demo/path/text.txt";

        public FileDataAccessTests()
        {
            _fileDataAccess = new FileDataAccess();
            _sharedFile = new SharedFile()
            {
                AuthorizedAccessGrantedTo = new List<string>(),
                HashId = FileHash,
                APIPath = APIPath, 
                FileName = "test.txt", 
                OwnerUserId = "TestOwner", 
                UploadDate = DateTime.Now
            };
        }

        [TestCleanup]
        public async Task TestCleanup()
        {
            await _fileDataAccess.FileDelete(FileHash);
        }

        [TestMethod]
        public async Task GetApiPathOfFileTest()
        {
            // arrange
            await _fileDataAccess.UploadFile(_sharedFile);

            // act
            var retrievedApiPath = await _fileDataAccess.GetApiPathOfFile(FileHash);

            // assert
            Assert.AreEqual(retrievedApiPath, APIPath);            
        }
    }
}
