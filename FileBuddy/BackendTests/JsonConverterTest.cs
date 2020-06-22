using NUnit.Framework;
using SharedResources.Services;
using System.Collections.Generic;

namespace SharedResourcesTests
{
    [TestFixture]
    public class JsonConverterTest
    {
        [Test]
        public void GetAsJsonTest()
        {
            // arrange
            const string expected = "{\"Name\":\"TestObject\",\"NestedList\":[{\"Name\":\"NestedElement\"}]}";
            var objectToParse = new TestParseObject();

            // act
            var result = JsonConverter.GetAsJson(objectToParse);

            // assert 
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetObjectFromJsonTest()
        {
            // arrange
            const string stringToConvert = "{\"Name\": \"TestObject\",\"NestedList\":[{\"Name\": \"NestedElement\"}]}";

            // act
            var result = JsonConverter.GetObjectFromJson<TestParseObject>(stringToConvert);

            // assert 
            Assert.AreEqual(new TestParseObject(), result);
        }
    }

    internal class TestParseObject
    {
        public string Name { get; } = "TestObject";
        public IList<NestedObject> NestedList { get; set; } = new List<NestedObject>()
        {
            new NestedObject()
        };

        public override bool Equals(object obj)
        {
            return obj is TestParseObject p && Name == p.Name;
        }

        public bool Equals(TestParseObject p)
        {
            if (p == null)
                return false;

            return Name == p.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }

    internal class NestedObject
    {
        public string Name { get; set; } = "NestedElement";

        public bool Equals(NestedObject p)
        {
            if (p == null)
                return false;

            return Name == p.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
