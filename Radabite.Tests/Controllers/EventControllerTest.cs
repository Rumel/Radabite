using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radabite.Client.WebClient.Controllers;

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
    }
}
