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



        [Authorize]
        public ActionResult GetPosts() 
        {
            DateTime startDate = new DateTime(2014, 1, 01);
            DateTime endDate = new DateTime(2014, 4, 1);
            if (Session["facebookUserToken"] != null)
            {
                var accessToken = Session["facebookUserToken"].ToString();
                var result = ServiceManager.Kernel.Get<IFacebookManager>().GetPosts(accessToken, startDate, endDate);
               // return new ContentResult { Content = result, ContentType = "application/json" };
                return new ContentResult { Content = "", ContentType = "application/json" };
            }
            else
            {
                return RedirectToAction("Login", "Account", new { returnUrl = "/feed/getposts" });
            }
        }

        /*
        public void GetPostsAsync()
        {
            AsyncManager.OutstandingOperations.Increment();
            DateTime startDate = new DateTime(2014, 2, 20);
            DateTime endDate = new DateTime(2014, 3, 13);
            var result = ServiceManager.Kernel.Get<IFacebookManager>().GetPostsAsync("cassey", "11", startDate, endDate);
            AsyncManager.OutstandingOperations.Decrement();
            AsyncManager.Parameters["result"] = result;
        }

        public ActionResult GetPostsCompleted(string result)
        {
            // return View();
            return new ContentResult { Content = result, ContentType = "application/json" };
        }
         * 
         */


    }
}
