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
                var postModel = ServiceManager.Kernel.Get<IFacebookManager>().GetPosts(accessToken, startDate, endDate);
                return View("Posts", postModel);
            }
            else
            {
                return RedirectToAction("Login", "Account", new { returnUrl = "/feed/getposts" });
            }
        }

    }
}
