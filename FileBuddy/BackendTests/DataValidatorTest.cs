using NUnit.Framework;
using SharedRessources.Services;
using System;

namespace SharedResourcesTests
{
    [TestFixture]
    public class DataValidatorTest
    {
        [Test]
        public void IsMailAddressValidReturnTrue()
        {
            // arrange
            const string validMailAddress = "test.user@something.awesome.com";

            // act
            var result = DataValidator.IsMailAddressValid(validMailAddress);

            // assert 
            Assert.IsTrue(result);
        }

        [Test]
        public void IsMailAddressValidThrowsException()
        {
            // arrange
            const string invalidMailAddress = "notAMailAddress";

            // act
            var result = DataValidator.IsMailAddressValid(invalidMailAddress);

            // assert   
            Assert.IsFalse(result);
        }
    }
}
