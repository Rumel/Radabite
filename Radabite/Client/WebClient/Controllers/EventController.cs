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
        public ActionResult Index(long eventId)
        {
            ViewBag.Message = "Event " + eventId.ToString();
			ViewBag.eventId = eventId;

            var eventRequest = ServiceManager.Kernel.Get<IEventManager>().GetById(eventId);

            // Used to test UI
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

		public ActionResult CreateEvent()
		{
			ViewBag.Message = "Create Event page.";

			return View();
		}

		public ActionResult DiscoverEvent()
		{
            ViewBag.Message = "Discover Event page.";

            //TODO: Add friends to UserProfile, so there will actually be friends in the db
            //var friends = ServiceManager.Kernel.Get<IUserManager>().GetById(userId).Friends;
            //dummy list of friends
            var friends = new List<User>()
			{
				new User{DisplayName = "1"},
				new User{DisplayName = "2"},
				new User{DisplayName = "3"}
			};

            return View(friends);
		}

        [HttpPost]
        public RedirectToRouteResult Create(string title, long startTime, long endTime/*, Location location*/)
        {
            var newEvent = new Event()
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
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
