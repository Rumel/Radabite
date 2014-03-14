using Radabite.Backend.Interfaces;
using RadabiteServiceManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Newtonsoft.Json.Linq;

namespace Radabite.Client.WebClient.Controllers
{
    public class FeedController : AsyncController
    {

        public ActionResult GetPostsCompleted(string result)
        {
            // return View();
            return new ContentResult { Content = result, ContentType = "application/json" };

        }

        public void GetPostsAsync()
        {
            DateTime startDate = new DateTime(2014, 2, 20);
            DateTime endDate = new DateTime(2014, 3, 13);
            var result = ServiceManager.Kernel.Get<IFacebookManager>().GetPostsAsync("cassey", "11", startDate, endDate);
            AsyncManager.Parameters["result"] = result;

        }

    }
}
