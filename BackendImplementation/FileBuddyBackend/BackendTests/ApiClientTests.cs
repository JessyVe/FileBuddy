using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedRessources.DataAccess.ApiAccess;
using SharedRessources.Dtos;
using System;
using System.Threading.Tasks;

namespace BackendTests
{
    [TestClass]
    public class ApiClientTests
    {
        private readonly ApiClient _client;
        private readonly AppUser _newTestUser1;
        private readonly AppUser _testUser2;

        public ApiClientTests()
        {
            _client = new ApiClient();

            _newTestUser1 = new AppUser()
            {
                AccountCreationDate = DateTime.Now,
                MailAddress = "test@user.com",
                Name = "TestUser1"
            };
            _testUser2 = new AppUser()
            {
                Name = "User2"
            };
        }

        [TestMethod]
        public async Task LoginWithMacAddress()
        {

        }

        [TestMethod]
        public async Task LoginWithMailAddress()
        {

        }

        [TestMethod]
        public async Task RegisterUser()
        {
            var registeredUserObject = await _client.RegisterUser(_newTestUser1);
            Assert.IsNotNull(_newTestUser1.Id);
        }
    }
}
