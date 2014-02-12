using Radabite.Backend.Database;
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

			Event dummy = new Event()
			{
				Id = 1,
				Title = "G-Ma's 9th birthday",
				StartTime = new DateTime(2014, 1, 1, 1, 1, 1),
				EndTime = new DateTime(2014, 1, 1, 1, 1, 2),
				IsPrivate = true,
				Description = "Happy Birthday Grandma",
				Location = new Location()
				{
					LocationId = 1,
					LocationName = "My house",
					Latitude = 1.01,
					Longitude = 1.01
				}
			};

            return View(dummy);
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
