using Ninject;
using Radabite.Backend.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radabite.Backend.Interfaces;
using RadabiteServiceManager;
using Radabite.Models;
using System.Net.Http;
using System.Text;

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

            //var eventRequest = ServiceManager.Kernel.Get<IEventManager>().GetById(eventId);
			Event eventRequest = null;

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
                        LocationName = "My house",
                        Latitude = 1.01,
                        Longitude = 1.01
                    }
                };
            }



//TESTING FOR GET and GET info
			var thing = ServiceManager.Kernel.Get<IFooCDNManager>().Get("c1485afb-d055-4f2f-a73e-c4e1bc22d2e9");
			var otherThing = ServiceManager.Kernel.Get<IFooCDNManager>().GetInfo("c1485afb-d055-4f2f-a73e-c4e1bc22d2e9");
			
			//POST
			//HttpContent content = new StreamContent("I am plain text");
			//var thirdThing = ServiceManager.Kernel.Get<IFooCDNManager>().Post("49971910-8aa5-4b8c-99fd-c37f6b98be92", content);

			var putThing = ServiceManager.Kernel.Get<IFooCDNManager>().Put("c1485afb-d055-4f2f-a73e-c4e1bc22d2e9", Radabite.Backend.Accessors.FooCDNAccessor.StorageType.Tape);


            return View(eventRequest);
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
        public RedirectToRouteResult Create(EventModel model)
        {
            var newEvent = new Event()
            {
                StartTime = new DateTime(model.StartTime.Ticks),
                EndTime = new DateTime(model.EndTime.Ticks),
                Location = new Location()
                {
                    LocationName = model.LocationName,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude
                },
                IsPrivate = model.IsPrivate,
                Title = model.Title,
                Description = model.Description
            };

            var result = ServiceManager.Kernel.Get<IEventManager>().Save(newEvent);

            if (result.Success)
            {
                return RedirectToAction("Index", new { userId = 123, eventId = result.Result.Id });
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
