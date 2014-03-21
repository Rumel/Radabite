using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radabite.Client.WebClient.Controllers;
using RadabiteServiceManager;
using Radabite.Backend.Interfaces;
using Radabite.Backend.Managers;
using Radabite.Tests.Mocks.Accessors;
using Radabite.Models;

namespace Radabite.Tests.Controllers
{
    [TestClass]
    public class EventControllerTest
    {
        EventModel getNewEventModel()
        {
            return new EventModel
            {
                Id = 3,
                Title = "My awesome event",
                StartTime = new DateTime(2012, 12, 12),
                EndTime = new DateTime(2012, 12, 12),
                IsPrivate = true,
                Description = "My awesome description",
                LocationName = "My house",
                Latitude = 1.01,
                Longitude = 1.01,
            };
        }
        
        [TestInitialize]
        public void Setup()
        {
            ServiceManager.Kernel.Rebind<IEventManager>().To<EventManager>();
            ServiceManager.Kernel.Rebind<IEventAccessor>().To<MockEventAccessor>().InSingletonScope();
            ServiceManager.Kernel.Rebind<IUserManager>().To<UserManager>();
            ServiceManager.Kernel.Rebind<IUserAccessor>().To<MockUserAccessor>().InSingletonScope();
        }

        [TestMethod]
        public void EventIndex()
        {
            EventController controller = new EventController();

            ViewResult result = controller.Index(123) as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DiscoverEvent()
        {
            EventController controller = new EventController();

            ViewResult result = controller.DiscoverEvent() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateEvent()
        {
            EventController controller = new EventController();

            var e = getNewEventModel();

            RedirectToRouteResult result = controller.Create(e) as RedirectToRouteResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteEvent()
        {
            EventController controller = new EventController();

            var e = getNewEventModel();

            RedirectToRouteResult result = controller.Delete(e) as RedirectToRouteResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdateEvent()
        {
            EventController controller = new EventController();

            var e = getNewEventModel();

            RedirectToRouteResult result = controller.Update(e) as RedirectToRouteResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void EventNotFound()
        {
            EventController controller = new EventController();

            ViewResult result = controller.EventNotFound() as ViewResult;

            Assert.IsNotNull(result);
        }
    }
}
