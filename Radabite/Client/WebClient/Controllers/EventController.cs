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
using Radabite.Client.WebClient.Models;

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

            if (eventRequest == null)
            {
                return Redirect("Event/EventNotFound");                    
            }

            //var posts = ServiceManager.Kernel.Get<IEventManager>().GetById(eventId);
            var posts = new List<Post>();

            var owner = new User
            {
                DisplayName = "Tom Jones",
                PhotoLink = "http://bit.ly/1nHr6dG"
            };

            var post1 = new Post()
            {
                From = owner,
                Message = "Guys please come to this event",
                SendTime = new DateTime(2013, 12, 12, 8, 59, 0),
                Likes = 0
            };

            var post2 = new Post()
            {
                From = owner,
                Message = "I have Doritos!",
                SendTime = new DateTime(2013, 12, 12, 9, 0, 0),
                Likes = 1
            };

            posts.Add(post1);
            posts.Add(post2);

            var eventViewModel = new EventModel()
            {
                Id = eventRequest.Id,
                Title = eventRequest.Title,
                StartTime = eventRequest.StartTime,
                EndTime = eventRequest.EndTime,
                IsPrivate = eventRequest.IsPrivate,
                Description = eventRequest.Description,
                LocationName = eventRequest.Location.LocationName,
                Latitude = eventRequest.Location.Latitude,
                Longitude = eventRequest.Location.Longitude,
                Posts = posts
            };

            return View(eventViewModel);
        }


		public ActionResult DiscoverEvent(string u)
		{
			ViewBag.Message = "Discover Event page.";

			var user = ServiceManager.Kernel.Get<IUserManager>().GetByUserName(u);

            var userModel = new UserModel
            {
                User = user
            };

            userModel.Friends = new List<User>
            {

            };

            userModel.Events = new List<Event> { };
            
			return View(userModel);
		}

        [HttpPost]
        public RedirectToRouteResult Delete(EventModel model)
        {
            var newEvent = new Event()
            {
                Id = model.Id,
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

        [HttpPost]
        public RedirectToRouteResult Update(EventModel model)
        {
            var newEvent = new Event()
            {
                Id = model.Id,
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

        public PartialViewResult _InviteFriends(string u)
        {
            var user = ServiceManager.Kernel.Get<IUserManager>().GetByUserName(u);

            var userModel = new UserModel
            {
                User = user,
                Friends = new List<User>()
                {
                    new User(){
                        DisplayName = "Clint Eastwood",
                        PhotoLink = "http://bit.ly/1hCIdbE"
                    },
                    new User(){
                        DisplayName = "Clift Eastwood",
                        PhotoLink = "http://bit.ly/1hCIdbE"
                    },
                    new User(){
                        DisplayName = "Clirt Eastwood",
                        PhotoLink = "http://bit.ly/1hCIdbE"
                    },
                    new User(){
                        DisplayName = "Clipt Eastwood",
                        PhotoLink = "http://bit.ly/1hCIdbE"
                    },
                    new User(){
                        DisplayName = "Clizt Eastwood",
                        PhotoLink = "http://bit.ly/1hCIdbE"
                    },
                    new User(){
                        DisplayName = "Clixt Eastwood",
                        PhotoLink = "http://bit.ly/1hCIdbE"
                    }
                }
            };

            return PartialView(userModel);
        }

        public ActionResult EventNotFound()
        {
            return View();
        }
    }
}
