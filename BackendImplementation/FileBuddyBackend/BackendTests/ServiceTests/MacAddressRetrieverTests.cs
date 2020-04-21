using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedRessources.Services;

namespace BackendTests.ServiceTests
{
    [TestClass]
    public class MacAddressRetrieverTests
    {
        [TestMethod]
        public void RetrieveMacAddress()
        {
            // arrange
            var actualMacAddress = "0A0027000002";

            // act
            var retrievedMacAddress = MacAddressRetriever.GetMacAddress();

            // assert
            Assert.AreEqual(actualMacAddress, retrievedMacAddress);
        }
    }
}
