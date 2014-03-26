using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Radabite.Models;
using Radabite.Backend.Database;
using Radabite.Backend.Interfaces;
using Radabite.Backend.Managers;
using Radabite.Tests.Mocks.Accessors;
using RadabiteServiceManager;

namespace Radabite.Tests.Managers
{
    [TestClass]
    public class UserManagerTest
    {
        private User CreateUser()
        {
            return new User
            {
                Id = 1,
                Age = 21,
                DisplayName = "Bilbo Baggins",
                Email = "bilbo@hobbiton.com",
                PhotoLink = "bilbo.jpg",
                SelfDescription = "I'm the most wonderfulest hobbit of all",
            };
        }

        [TestInitialize]
        public void Setup()
        {
            ServiceManager.Kernel.Rebind<IUserManager>().To<UserManager>();
            ServiceManager.Kernel.Rebind<IUserAccessor>().To<MockUserAccessor>().InSingletonScope();
        }

        [TestMethod]
        public void SaveTest()
        {
            var u = CreateUser();
            var result = ServiceManager.Kernel.Get<IUserManager>().Save(u);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Result.Id > 0);
        }

        [TestMethod]
        public void GetAllTest()
        {
            Assert.AreEqual(5, ServiceManager.Kernel.Get<IUserManager>().GetAll().Count());
        }

        [TestMethod]
        public void GetByIdTest()
        {
            Assert.AreEqual(1, ServiceManager.Kernel.Get<IUserManager>().GetById(1).Id);
        }

        [TestMethod]
        public void GetByUserNameTest()
        {   
            var name = "Dalton";
            var actual = ServiceManager.Kernel.Get<IUserManager>().GetByUserName(name);
            Assert.AreNotEqual(null, actual.FacebookProfile);
        }
    }
}
