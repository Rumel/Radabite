using Radabite.Backend.Interfaces;
using RadabiteServiceManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Newtonsoft.Json.Linq;
using Radabite.Backend.Database; 

namespace Radabite.Client.WebClient.Controllers
{
    public class FeedController : AsyncController
    {

        [Authorize]
        public ActionResult GetPosts() 
        {
            DateTime startDate = new DateTime(2014, 1, 01);
            DateTime endDate = new DateTime(2014, 4, 1);
            User user = ServiceManager.Kernel.Get<IUserManager>().GetByUserName(User.Identity.Name);
            var getResult = ServiceManager.Kernel.Get<IFacebookManager>().GetPosts(user, startDate, endDate);

            if (getResult.hasErrors == false)
            {
                return View("Posts", getResult.posts);
            }
            else
            {
                return RedirectToAction("Login", "Account", new { returnUrl = "/feed/getposts" });
            }
            
        }

        [Authorize]
        public ActionResult GetPhotos()
        {
            DateTime startDate = new DateTime(2014, 4, 01);
            DateTime endDate = new DateTime(2014, 4, 15);
            User user = ServiceManager.Kernel.Get<IUserManager>().GetByUserName(User.Identity.Name);
            var getResult = ServiceManager.Kernel.Get<IFacebookManager>().GetPhotos(user, startDate, endDate);

            if (getResult.hasErrors == false)
            {
                return Content("good!");
            }
            else
            {
                return RedirectToAction("Login", "Account", new { returnUrl = "/feed/getphotos" });
            }
        }

        [Authorize]
        public ActionResult GetTweets()
        {
            DateTime startDate = new DateTime(2014, 1, 01);
            DateTime endDate = new DateTime(2014, 4, 30);
            User user = ServiceManager.Kernel.Get<IUserManager>().GetByUserName(User.Identity.Name);
            var getResult = ServiceManager.Kernel.Get<ITwitterManager>().GetTweets(user, startDate, endDate);

            if (getResult.hasErrors == false)
            {
                return View("Tweets", getResult.posts);
            }
            else
            {
                return RedirectToAction("Login", "Account", new { returnUrl = "/feed/getposts" });
            }

        }

        [Authorize]
        public ActionResult Invite(string message)
        {
                User user = ServiceManager.Kernel.Get<IUserManager>().GetByUserName(User.Identity.Name);
                var accessToken = user.FacebookToken;
                    var postResponse = ServiceManager.Kernel.Get<IFacebookManager>().PublishStatus(user, message);
                    if (postResponse.hasErrors)
                    {
                        return Content(postResponse.errorMessage.ToString());
                    }
                    else 
                    { 
                        return Content("Posted!");
                    }
                //return RedirectToAction("Login", "Account", new { returnUrl = "/feed/invite" });;
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
