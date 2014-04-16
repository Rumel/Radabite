using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radabite.Backend.Managers;
using Radabite.Backend.Database;

namespace Radabite.Tests.Managers
{
    [TestClass]
    public class FacebookManagerTest
    {
        [TestMethod]
        public void GetPosts()
        {
            var manager = new FacebookManager();
            var expected = manager.GetPosts(new User(), new DateTime(2014, 3, 3), new DateTime(2014, 3, 22));
            Assert.IsNotNull(expected);
        }
    }
}
