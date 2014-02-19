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
    public class UserProfileControllerTest
    {
        [TestMethod]
        public void ProfileIndex()
        {
            UserProfileController controller = new UserProfileController();

            ViewResult result = controller.Index(123) as ViewResult;

            Assert.IsNotNull(result);
        }
    }
}
