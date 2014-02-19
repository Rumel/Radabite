﻿using Ninject;
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
                    LocationId = 123
                    /*Location = new Location()
                    {
                        LocationId = 1,
                        LocationName = "My house",
                        Latitude = 1.01,
                        Longitude = 1.01
                    }*/
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
            /*
            TODO No current way to save locations, just using dummy ID for now
            var newLocation = new Location()
            {
                LocationName = "My house",
                Latitude = 1.01,
                Longitude = 1.01
            };

            var locationSave = ServiceManager.Kernel.Get<IEventManager>().Save(newLocation)
             */
            var newEvent = new Event()
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                LocationId = 123,
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
