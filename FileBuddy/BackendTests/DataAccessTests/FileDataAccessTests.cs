using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedRessources.DataAccess.FileDataAccess;
using SharedRessources.Dtos;
using System;
using System.Collections.Generic;

namespace BackendTests.DataAccessTests
{
    [TestClass]
    public class FileDataAccessTests
    {
        private readonly IFileDataAccess _fileDataAccess;
        private SharedFile _sharedFile;
        private IList<int> _authorizedAccessGrantedTo;

        private const string APIPath = "demo/path/text.txt";

        public FileDataAccessTests()
        {
            _fileDataAccess = new FileDataAccess();
            _sharedFile = new SharedFile()
            {
                SharedFileName = "test.txt",
                ApiPath = APIPath, 
                OwnerUserId = 1,
                UploadDate = DateTime.Now
            };

            _authorizedAccessGrantedTo = new List<int>() { 1 };
        }

        [TestMethod]
        public void UploadFileTest()
        {
            // act
           _fileDataAccess.UploadFile(_sharedFile, _authorizedAccessGrantedTo);
            var newFileId = _sharedFile.Id;

            // assert   
            Assert.AreNotEqual(newFileId, 0);
        }

        [TestMethod]
        public void GetApiPathOfFileTest()
        {
            // arrange          

            // act

            // assert          
        }
    }
}
