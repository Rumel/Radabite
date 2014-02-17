using Radabite.Backend.Accessors;
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
        EventAccessor eventAccessor = new EventAccessor();

        //
        // GET: /Event/
        public ActionResult Index(long eventId, long userId)
        {
			ViewBag.Message = "Event " + eventId.ToString();
			ViewBag.eventId = eventId;
			ViewBag.userId = userId;

            Event eventRequest = (Event) eventAccessor.GetAll().Where(userEvent => userEvent.Id == eventId);

            if (eventRequest == null)
            {
                eventRequest = new Event()
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
            }

            return View(eventRequest);
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

        [HttpPost]
        public RedirectToRouteResult Create(string title, long startTime, long endTime/*, Location location*/)
        {
            Event newEvent = new Event();
            newEvent.Id = 321;
            newEvent.StartTime = new DateTime(startTime);
            newEvent.EndTime = new DateTime(endTime);
            newEvent.Location = new Location()
            {
                LocationId = 1,
                LocationName = "My house",
                Latitude = 1.01,
                Longitude = 1.01
            };
            newEvent.IsPrivate = false;
            newEvent.Title = title;


            //eventAccessor.Save(newEvent);

            return RedirectToAction("Index", new { userId = 123, eventId = 321 });
        }
    }
}
