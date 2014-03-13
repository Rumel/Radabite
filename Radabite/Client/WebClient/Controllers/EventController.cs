﻿using Ninject;
using Radabite.Backend.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radabite.Backend.Interfaces;
using RadabiteServiceManager;
using Radabite.Models;

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
                return Redirect("Event/EventNotFound");                    
            }

            var eventViewModel = new EventModel()
            {
                Title = eventRequest.Title,
                StartTime = eventRequest.StartTime,
                EndTime = eventRequest.EndTime,
                IsPrivate = eventRequest.IsPrivate,
                Description = eventRequest.Description,
                // TODO retrieve locations
                LocationName = "My house",
                Latitude = 1.01,
                Longitude = 1.01,
           };

            return View(eventViewModel);
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
        public RedirectToRouteResult Delete(EventModel model)
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
                Description = model.Description,
                // Soft delete entry
                IsActive = false
            };

            var result = ServiceManager.Kernel.Get<IEventManager>().Save(newEvent);

            if (result.Success)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                throw new Exception();
            }
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
                Description = model.Description,
                IsActive = model.IsActive
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

        public ActionResult EventNotFound()
        {
            return View();
        }
    }
}
