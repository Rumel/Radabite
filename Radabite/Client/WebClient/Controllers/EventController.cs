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

            var invitationList = eventRequest.Guests.Where(x => x.Response == ResponseType.Accepted);
            var guestList = new List<User>();
            foreach(var i in invitationList)
            {
                guestList.Add(ServiceManager.Kernel.Get<IUserManager>().GetById(i.GuestId));
            }

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
                Posts = posts,
                Owner = eventRequest.Owner,
                Guests = guestList
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

            userModel.Friends = ServiceManager.Kernel.Get<IUserManager>().GetAll().ToList();

            userModel.DiscoverEvents = ServiceManager.Kernel.Get<IEventManager>().GetAll().ToList();

            userModel.EventInvitations = ServiceManager.Kernel.Get<IEventManager>().GetByGuestId(user.Id);
            
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
                IsActive = false,
                Owner = model.Owner
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
        [Authorize]
        public RedirectToRouteResult Create(EventModel model)
        {
            var user = ServiceManager.Kernel.Get<IUserManager>().GetByUserName(User.Identity.Name);

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
                IsActive = model.IsActive,
                Owner = user
            };

            var result = ServiceManager.Kernel.Get<IEventManager>().Save(newEvent);

            if (result.Success)
            {
                return RedirectToAction("Index", new { userId = user.Id, eventId = result.Result.Id });
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
                IsActive = model.IsActive,
                Owner = model.Owner
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

        public PartialViewResult _InviteFriends(string u, long eventId)
        {
            User user = ServiceManager.Kernel.Get<IUserManager>().GetByUserName(u);
            user.Friends = ServiceManager.Kernel.Get<IUserManager>().GetAll().ToList();


            var invitationModel = new InvitationModel
            {
                User = user,
                EventId = eventId
            };

            return PartialView(invitationModel);
        }

        [HttpPost]
        public void Invite(List<String> friends, long eventId)
        {
            var e = ServiceManager.Kernel.Get<IEventManager>().GetById(eventId);
            foreach (var f in friends)
            {
                e.Guests.Add(new Invitation 
                {
                    Guest = ServiceManager.Kernel.Get<IUserManager>().GetById(long.Parse(f)),
                    GuestId = long.Parse(f),
                    Response = ResponseType.WaitingReply
                });
            }
            ServiceManager.Kernel.Get<IEventManager>().Save(e);

            return;
        }

        [HttpPost]
        public void RespondToInvitation(string userId, string eventId, string response)
        {
            var e = ServiceManager.Kernel.Get<IEventManager>().GetById(long.Parse(eventId));
            var r = ResponseType.WaitingReply;
            if (response.Equals("Accept"))
                r = ResponseType.Accepted;
            else if (response.Equals("Decline"))
                r = ResponseType.Rejected;

            e.Guests.FirstOrDefault(g => g.GuestId == long.Parse(userId)).Response = r;

            ServiceManager.Kernel.Get<IEventManager>().Save(e);

            return;
        }

        public ActionResult EventNotFound()
        {
            return View();
        }
    }
}
