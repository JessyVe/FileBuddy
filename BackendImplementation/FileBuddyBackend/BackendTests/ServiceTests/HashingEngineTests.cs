using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedRessources.Dtos;
using SharedRessources.Services;
using System;
using System.Collections.Generic;

namespace BackendTests.ServiceTests
{
    [TestClass]
    public class HashingEngineTests
    {
        private readonly HashingEngine _fileHashingEngine;
        private readonly HashingEngine _userHashingEngine;

        public HashingEngineTests()
        {
            _fileHashingEngine = new FileHashingEngine();
            _userHashingEngine = new UserHashingEngine();
        }

        private static IEnumerable<SharedFile> SharedTestFiles
        {
            get
            {
                return new List<SharedFile>(){
                    new SharedFile()
                    {
                        FileName = "Readme.md",
                        OwnerUserId =  "TestUser1" ,
                        UploadDate = DateTime.Parse("2020-01-01 15:40:00")
                    },
                    new SharedFile()
                    {
                        FileName = "Readme2.md",
                        OwnerUserId ="TestUser2",
                        UploadDate = DateTime.Parse("2020-04-11 09:48:05")
                    }
                };
            }
        }

        private static IEnumerable<User> TestUser
        {
            get
            {
                return new List<User>(){
                    new User()
                    {
                        Name = "User1",
                        AccountCreationDate = DateTime.Parse("2020-04-11 09:48:05")
                    },
                    new User()
                    {
                        Name = "User2",
                        AccountCreationDate = DateTime.Parse("2019-03-01 16:02:45")
                    }
                };
            }
        }

        private static User[] IdenticalTestUser
        {
            get
            {
                return new User[] {
                    new User()
                    {
                        Name = "User1",
                        AccountCreationDate = DateTime.Parse("2020-04-11 09:48:05")
                    },
                    new User()
                    {
                        Name = "User1",
                        AccountCreationDate = DateTime.Parse("2020-04-11 09:48:05")
                    }
                };
            }
        }

        [TestMethod]
        public void GenerateHashForSharedFile()
        {
            foreach (var sharedFile in SharedTestFiles)
            {
                _fileHashingEngine.SetHash(sharedFile);
                Assert.IsTrue(_fileHashingEngine.VerifyHash(sharedFile));
            }
        }

        [TestMethod]
        public void GenerateHashForUser()
        {
            foreach (var user in TestUser)
            {
                _userHashingEngine.SetHash(user);
                Assert.IsTrue(_userHashingEngine.VerifyHash(user));
            }
        }

        [TestMethod]
        public void CompareIdenticalUsersHash()
        {
            var user1 = IdenticalTestUser[0];
            var user2 = IdenticalTestUser[1];

            _userHashingEngine.SetHash(user1);
            _userHashingEngine.SetHash(user2);

            Assert.IsTrue(user1.HashId != user2.HashId);
        }
    }
}
