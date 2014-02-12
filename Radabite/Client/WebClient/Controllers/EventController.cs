using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Radabite.Client.WebClient.Controllers
{
    public class EventController : Controller
    {
        //
        // GET: /Event/

        public ActionResult Index(long eventId)
        {
			ViewBag.Message = "Event " + eventId.ToString();
			ViewBag.eventId = eventId;

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
