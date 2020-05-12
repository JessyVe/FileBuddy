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
        private IList<string> _authorizedAccessGrantedTo;

        private const string FileHash = "HashId1";
        private const string APIPath = "demo/path/text.txt";

        public FileDataAccessTests()
        {
            _fileDataAccess = new FileDataAccess();
            _sharedFile = new SharedFile()
            {
                ApiPath = APIPath, 
                SharedFileName = "test.txt"
            };

            _authorizedAccessGrantedTo = new List<string>()
            {
                "User1", "User2", "User3"
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
            await _fileDataAccess.UploadFile(_sharedFile, _authorizedAccessGrantedTo);

            // act
            var retrievedApiPath = await _fileDataAccess.GetApiPathOfFile(FileHash);

            // assert
            Assert.AreEqual(retrievedApiPath, APIPath);            
        }
    }
}
