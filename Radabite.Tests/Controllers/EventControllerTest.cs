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
using Moq;
using Radabite.Backend.Database;

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
            ServiceManager.Kernel.Rebind<IFooCDNManager>().To<FooCDNManager>();
            ServiceManager.Kernel.Rebind<IFooCDNAccessor>().To<MockFooCDNAccessor>().InSingletonScope();
        }

        [TestMethod]
        public void EventIndex()
        {
            EventController controller = new EventController();

			var mock = new Mock<ControllerContext>();
			mock.SetupGet(x => x.HttpContext.User.Identity.Name).Returns("Bob");
			mock.SetupGet(x => x.HttpContext.Request.IsAuthenticated).Returns(true);
			controller.ControllerContext = mock.Object;

			ViewResult result = controller.Index(123) as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetImg()
        {
            EventController controller = new EventController();

            FileContentResult result = controller.GetImg("1485afb-d055-4f2f-a73e-c4e1bc22d2e9", "jpg") as FileContentResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DiscoverEvent()
        {
            EventController controller = new EventController();

            ViewResult result = controller.DiscoverEvent("abcd") as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateEvent()
        {
            var controller = new EventController();

            var e = getNewEventModel();

            var mock = new Mock<ControllerContext>();
            mock.SetupGet(x => x.HttpContext.User.Identity.Name).Returns("Bob");
            mock.SetupGet(x => x.HttpContext.Request.IsAuthenticated).Returns(true);
            controller.ControllerContext = mock.Object;

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
        public void Invite()
        {
            EventController controller = new EventController();

            var friends = new List<String>
            {
                "1", "2", "3", "4"
            };

            var mock = new Mock<ControllerContext>();
            mock.SetupGet(x => x.HttpContext.User.Identity.Name).Returns("Bob");
            mock.SetupGet(x => x.HttpContext.Request.IsAuthenticated).Returns(true);
            controller.ControllerContext = mock.Object;

            PartialViewResult result = controller.Invite(friends, 1) as PartialViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void RespondToInvitation()
        {
            EventController controller = new EventController();

            var friends = new List<String>
            {
                "1", "2", "3", "4"
            };

            PartialViewResult result = controller.RespondToInvitation("1", "2", "Accept") as PartialViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PostFromRadabite()
        {
            EventController controller = new EventController();

            PartialViewResult result = controller.PostFromRadabite("1", "2", "Message", false) as PartialViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CommentFromRadabite()
        {
            EventController controller = new EventController();

            PartialViewResult result = controller.CommentFromRadabite("1", "2", "2", "Message") as PartialViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Vote()
        {
            EventController controller = new EventController();

            PartialViewResult result = controller.Vote("1", "Name", DateTime.Now.ToString()) as PartialViewResult;

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
