using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Radabite.WebClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult UserProfile(long userId)
        {
            ViewBag.Message = userId.ToString() + "'s profile page.";
            ViewBag.userId = userId;

            return View();
        }

        public ActionResult CreateEvent(long userId)
        {
            ViewBag.Message = userId.ToString() + "'s Create Event page.";
            ViewBag.userId = userId;

            return View();
        }
        
        public ActionResult DiscoverEvent(long userId)
        {
            ViewBag.Message = userId.ToString() + "'s Discover Event page.";
            ViewBag.userId = userId;

            return View();
        }
    }
}
