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
            var validMailAddress = "test.user@something.awesome.com";

            // act
            var result = DataValidator.IsMailAddressValid(validMailAddress);

            // assert 
            Assert.IsTrue(result);
        }

        [Test]
        public void IsMailAddressValidThrowsException()
        {
            // arrange
            var invalidMailAddress = "notAMailAddress";

            // assert   
            Assert.Throws<FormatException>(() => 
                   DataValidator.IsMailAddressValid(invalidMailAddress));
        }
    }
}
