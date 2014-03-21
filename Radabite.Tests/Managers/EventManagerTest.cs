using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Radabite.Backend.Database;
using Radabite.Backend.Interfaces;
using Radabite.Backend.Managers;
using Radabite.Tests.Mocks.Accessors;
using RadabiteServiceManager;

namespace Radabite.Tests.Managers
{
    [TestClass]
    public class EventManagerTest
    {
        private Event CreateNewEvent()
        {
            return new Event
            {
                Description = "My cool description",
                EndTime = DateTime.Now + TimeSpan.FromHours(2),
                StartTime = DateTime.Now,
                IsPrivate = false,
                Location = new Location
                {
                    Latitude = 0,
                    Longitude = 0,
                    LocationName = "My House"
                },
                Title = "PARTY",
                IsActive = true
            };
        }

        [TestInitialize]
        public void Setup()
        {
            ServiceManager.Kernel.Rebind<IEventManager>().To<EventManager>();
            ServiceManager.Kernel.Rebind<IEventAccessor>().To<MockEventAccessor>().InSingletonScope();
        }

        [TestMethod]
        public void SaveTest()
        {
            var e = CreateNewEvent();
            var result = ServiceManager.Kernel.Get<IEventManager>().Save(e);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Result.Id == 1);
        }

        [TestMethod]
        public void GetAllTest()
        {
            Assert.AreEqual(5, ServiceManager.Kernel.Get<IEventManager>().GetAll().Count());
        }

        [TestMethod]
        public void GetById()
        {
            Assert.AreEqual(1, ServiceManager.Kernel.Get<IEventManager>().GetById(1).Id);
        }
    }
}
