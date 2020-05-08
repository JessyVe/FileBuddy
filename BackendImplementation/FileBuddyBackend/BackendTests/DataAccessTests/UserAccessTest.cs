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

        private readonly FullUserData _testUser1;
        private readonly FullUserData _testUser2;

        public UserAccessTest()
        {
            _userAccess = new UserAccess();
            _authentificationService = new AuthentificationService();

            _testUser1 = new FullUserData()
            {
                HashId = UserHashId1, 
                Name = "User1"
            };
            _testUser2 = new FullUserData()
            {
                HashId = UserHashId2,
                Name = "User2"
            };
        }

        [TestInitialize]
        public async Task TestInitialize()
        {
            await _authentificationService.RegisterUser(_testUser1);
            await _authentificationService.RegisterUser(_testUser2);
        }

        [TestCleanup]
        public async Task TestCleanup()
        {
            await _userAccess.DeleteUser(UserHashId1);
            await _userAccess.DeleteUser(UserHashId2);
        }

        [TestMethod]
        public async Task LoadAllUsersFromDatabaseTest()
        {
            // act
            var allUsers = await _userAccess.LoadAllUsersFromDatabase();

            // assert
            Assert.IsTrue(allUsers.Count == 2);
        }
    }
}
