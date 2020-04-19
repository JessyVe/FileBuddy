using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedRessources.DataAccess;
using SharedRessources.Dtos;
using System;
using System.Threading.Tasks;

namespace BackendTests.DatabaseAccessTests
{
    [TestClass]
    public class FirebaseDatabaseAccessTests
    {
        private readonly FirebaseDatabaseAccess _firebaseAccess;

        private readonly User _newTestUser1;
        private readonly User _testUser2;

        public FirebaseDatabaseAccessTests()
        {
            _firebaseAccess = new FirebaseDatabaseAccess();
            _newTestUser1 = new User()
            {
                AccountCreationDate = DateTime.Now, 
                MailAddress = "test@user.com", 
                Name = "TestUser1"
            };
            _testUser2 = new User()
            {
                HashId = "User2"
            };
        }

        [TestMethod]
        public async Task LoginWithMacAddress()
        {
           
        }

        [TestMethod]
        public void LoginWithMailAddress()
        {

        }

        [TestMethod]
        public async Task RegisterUser()
        {
            var registeredUserObject = await _firebaseAccess.RegisterUser(_newTestUser1);
            Assert.IsTrue(!string.IsNullOrEmpty(registeredUserObject.HashId) && registeredUserObject.Seed != default);
        }

        [TestMethod]
        public void UploadFiles()
        {
            //_firebaseAccess.UploadFiles(_testUser1.HashId);
        }

        [TestMethod]
        public void DownloadFiles()
        {

        }

        [TestMethod]
        public void FetchAvailableFiles()
        {

        }

        [TestMethod]
        public void UpdateUserInformation()
        {

        }

        [TestMethod]
        public async Task GetUserInformation()
        {
            // setup new user
            var registeredUserObject = await _firebaseAccess.RegisterUser(_newTestUser1);

            // retrieve user
            var retrievedUserInformation = await _firebaseAccess.GetUserInformation(registeredUserObject.HashId);

            // verify
            Assert.IsTrue(registeredUserObject.CompareTo(retrievedUserInformation) == 0);
        }

        [TestMethod]
        public void UpdateGroupInformationOfUser()
        {

        }

        [TestMethod]
        public void GetGroupInformationOfUser()
        {

        }
    }
}
