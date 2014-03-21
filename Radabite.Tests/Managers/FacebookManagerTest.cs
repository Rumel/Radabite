using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radabite.Backend.Managers;

namespace Radabite.Tests.Managers
{
    [TestClass]
    public class FacebookManagerTest
    {
        [TestMethod]
        public void GetPosts()
        {
            //TODO - this isn't even a test.
            var manager = new FacebookManager();
            var expected = manager.GetPosts("", "", "", new DateTime(2014, 3, 3), new DateTime(2014, 3, 22));
            Assert.IsNotNull(expected);
        }
    }
}
