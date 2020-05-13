using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedRessources.DataAccess.Authentification;
using SharedRessources.DataAccess.UserAccess;
using SharedRessources.Dtos;
using System.Threading.Tasks;

namespace BackendTests.DataAccessTests
{
    [TestClass]
    public class UserAccessTest
    {
        private readonly IUserAccess _userAccess;
        private readonly IAuthentificationService _authentificationService;

        private const string UserHashId1 = "userHash1";
        private const string UserHashId2 = "userHash2";

        private readonly AppUser _testUser1;
        private readonly AppUser _testUser2;

        public UserAccessTest()
        {
            _userAccess = new UserAccess();
            _authentificationService = new AuthentificationService();

            _testUser1 = new AppUser()
            {
                Name = "User1"
            };
            _testUser2 = new AppUser()
            {
                Name = "User2"
            };
        }

        [TestInitialize]
        public void TestInitialize()
        {
             _authentificationService.RegisterUser(_testUser1);
             _authentificationService.RegisterUser(_testUser2);
        }

        [TestCleanup]
        public void TestCleanup()
        {
             _userAccess.DeleteUser(UserHashId1);
             _userAccess.DeleteUser(UserHashId2);
        }

        [TestMethod]
        public void LoadAllUsersFromDatabaseTest()
        {
            // act
            var allUsers =  _userAccess.LoadAllUsersFromDatabase();

            // assert
           // Assert.IsTrue(allUsers.Count == 2);
        }
    }
}
