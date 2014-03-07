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
            manager.PracticeGet();

        }
    }
}
