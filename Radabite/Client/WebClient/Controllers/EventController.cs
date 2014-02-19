using Ninject;
using Radabite.Backend.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radabite.Backend.Interfaces;
using RadabiteServiceManager;

namespace Radabite.Client.WebClient.Controllers
{
    public class EventController : Controller
    {

        //
        // GET: /Event/
        public ActionResult Index(long eventId, long userId)
        {
            ViewBag.Message = "Event " + eventId;
			ViewBag.eventId = eventId;
			ViewBag.userId = userId;
            
            var eventRequest = ServiceManager.Kernel.Get<IEventManager>().GetById(eventId);

            // I assume this is being used to test the UI
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
			ViewBag.Message = userId + "'s Create Event page.";
			ViewBag.userId = userId;

			return View();
		}

		public ActionResult DiscoverEvent(long userId)
		{
			ViewBag.Message = userId + "'s Discover Event page.";
			ViewBag.userId = userId;

			var friends = ServiceManager.Kernel.Get<IUserManager>().GetById(userId).Friends;
			
			return View(friends);
		}

        [HttpPost]
        public RedirectToRouteResult Create(string title, long startTime, long endTime/*, Location location*/)
        {
            var newEvent = new Event()
            {
                StartTime = new DateTime(startTime),
                EndTime = new DateTime(endTime),
                Location = new Location()
                {
                    LocationName = "My house",
                    Latitude = 1.01,
                    Longitude = 1.01
                },
                IsPrivate = false,
                Title = title
            };

            var result = ServiceManager.Kernel.Get<IEventManager>().Save(newEvent);

            if (result.Success)
            {
                return RedirectToAction("Index", new {userId = 123, eventId = result.Result.Id});
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
