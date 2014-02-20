﻿using System;
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

namespace Radabite.Tests.Controllers
{
    [TestClass]
    public class EventControllerTest
    {
        [TestMethod]
        public void EventIndex()
        {
            EventController controller = new EventController();

            ViewResult result = controller.Index(123, 234) as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateEvent()
        {
            EventController controller = new EventController();

            ViewResult result = controller.CreateEvent(123) as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DiscoverEvent()
        {
            EventController controller = new EventController();

            ViewResult result = controller.DiscoverEvent(123) as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestInitialize]
        public void Setup()
        {
            ServiceManager.Kernel.Rebind<IEventManager>().To<EventManager>();
            ServiceManager.Kernel.Rebind<IEventAccessor>().To<MockEventAccessor>().InSingletonScope();
            ServiceManager.Kernel.Rebind<IUserManager>().To<UserManager>();
            ServiceManager.Kernel.Rebind<IUserAccessor>().To<MockUserAccessor>().InSingletonScope();
        }
    }
}
