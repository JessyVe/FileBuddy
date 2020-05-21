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
        private readonly AppUser _newTestUser1;
        private readonly string _macAddress = "";

        public ApiClientTests()
        {
            _newTestUser1 = new AppUser()
            {
                Id = 0,
                AccountCreationDate = DateTime.Now,
                MailAddress = "test@user.com" + DateTime.Now,
                Name = "TestUser1",
                Password = "supersecure"
            };
        }

        [TestMethod]
        public async Task LoginWithMacAddress()
        {
            var registeredUserObject = await ApiClient.Instance.LoginWithMailAddress(_newTestUser1);
            Assert.IsNotNull(_newTestUser1.Id);
        }

        [TestMethod]
        public async Task LoginWithMailAddress()
        {
            var registeredUserObject = await ApiClient.Instance.LoginWithMacAddress(_macAddress);
            Assert.IsNotNull(_newTestUser1.Id);
        }

        [TestMethod]
        public async Task RegisterUser()
        {
            var registeredUserObject = await ApiClient.Instance.RegisterUser(_newTestUser1);
            Assert.AreNotEqual(_newTestUser1.Id, registeredUserObject.Id);
        }
    }
}
