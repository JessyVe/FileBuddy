using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedRessources.DataAccess.ApiAccess;
using SharedRessources.Dtos;
using System;

namespace BackendTests
{
    [TestClass]
    public class ApiClientTests
    {
        private readonly ApiClient _client;
        private readonly AppUser _newTestUser1;

        public ApiClientTests()
        {
            _client = new ApiClient();

            _newTestUser1 = new AppUser()
            {
                AccountCreationDate = DateTime.Now,
                MailAddress = "test@user.com",
                Name = "TestUser1"
            };
        }

        [TestMethod]
        public void LoginWithMacAddress()
        {

        }

        [TestMethod]
        public void LoginWithMailAddress()
        {

        }

        [TestMethod]
        public void RegisterUser()
        {
            var registeredUserObject = _client.RegisterUser(_newTestUser1);
            Assert.IsNotNull(_newTestUser1.Id);
        }
    }
}
