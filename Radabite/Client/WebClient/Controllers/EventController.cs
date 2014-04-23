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
using System.Net.Http;
using System.Text;
using Radabite.Client.WebClient.Models;
//using WebMatrix.WebData;

using Radabite.Backend.Helpers;
using System.Configuration;

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

            var currentUser = ServiceManager.Kernel.Get<IUserManager>().GetByUserName(User.Identity.Name);

            var eventRequest = ServiceManager.Kernel.Get<IEventManager>().GetById(eventId);

            if (eventRequest == null)
            {
                return Redirect("Event/EventNotFound");                    
            }
                        
            foreach(var i in eventRequest.Guests)
            {
                i.Guest = ServiceManager.Kernel.Get<IUserManager>().GetById(i.GuestId);
            }

            foreach (var p in eventRequest.Posts)
            {
                p.From = ServiceManager.Kernel.Get<IUserManager>().GetById(p.FromId);
            }
            
            foreach(var i in eventRequest.Guests.Where(x => x.Response == ResponseType.Accepted))
            {
                var postModel = ServiceManager.Kernel.Get<IFacebookManager>().GetPosts(i.Guest, eventRequest.StartTime, eventRequest.EndTime);
                foreach (var p in postModel.posts)
                {
                    eventRequest.Posts.Add(new Post
                    {
                        From = i.Guest,
                        FromId = i.GuestId,
                        Message = p.message,
                        SendTime = p.created_time.DateTime
                    });
                }

                var photoPostModel = ServiceManager.Kernel.Get<IFacebookManager>().GetPhotos(i.Guest, eventRequest.StartTime, eventRequest.EndTime);
                foreach (var p in photoPostModel.posts)
                {
                    if (p.fromName == i.Guest.DisplayName)
                    {  
                        eventRequest.Posts.Add(new Post{
                            From = i.Guest,
                            FromId = i.GuestId,
                            Message = p.message,
                            SendTime = p.created_time.DateTime
                        });
                    }
                }
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
                Posts = eventRequest.Posts.OrderBy(p => p.SendTime).Reverse().ToList(),
                Owner = eventRequest.Owner,
                CurrentUser = currentUser,
                Guests = eventRequest.Guests.ToList()
            };

            eventViewModel.CurrentUser.Friends = ServiceManager.Kernel.Get<IUserManager>().GetAll().ToList();

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
				StorageLocation = Backend.Accessors.FooCDNAccessor.StorageType.Tape,
                Owner = user,
                Posts = new List<Post>()
            };

            ServiceManager.Kernel.Get<IEventManager>().Save(newEvent);

            newEvent.Guests = new List<Invitation>()
            {
                new Invitation
                {
                    Guest = user,
                    GuestId = user.Id,
                    Response = ResponseType.Accepted
                }
            };

            var result = ServiceManager.Kernel.Get<IEventManager>().Save(newEvent);

            if (model.ToFacebook)
            {
                ServiceManager.Kernel.Get<IFacebookManager>().PublishStatus(user, "Hey guys, I just created an event on Radabite at http://localhost:3000/Event?eventId=" + result.Result.Id + ", make sure to check it out!");
            }

            if (result.Success)
            {
                return RedirectToAction("Index", new { eventId = result.Result.Id });
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
                return RedirectToAction("Index", new { eventId = result.Result.Id });
            }
            else
            {
                throw new Exception();
            }
        }

        [HttpPost]
        public PartialViewResult Invite(List<String> friends, long eventId)
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

            var eventModel = new EventModel
            {
                CurrentUser = ServiceManager.Kernel.Get<IUserManager>().GetByUserName(User.Identity.Name),
                Guests = e.Guests.ToList()
            };

            return PartialView("_InvitationPanel", eventModel);
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

        [HttpPost]
        public PartialViewResult PostFromRadabite(string eventId, string username, string message, bool toFacebook)
        {
            var e = ServiceManager.Kernel.Get<IEventManager>().GetById(long.Parse(eventId));
            var u = ServiceManager.Kernel.Get<IUserManager>().GetByUserName(username);
            var newPost = new Post 
            {
                From = u,
                FromId = u.Id,
                Message = message,
                SendTime = DateTime.Now,
                Likes = 0
            };
            
            e.Posts.Add(newPost);
            
            ServiceManager.Kernel.Get<IEventManager>().Save(e);

            if (toFacebook)
            {
                ServiceManager.Kernel.Get<IFacebookManager>().PublishStatus(u, message);
            }


            var dbPosts = e.Posts;
            foreach (var i in e.Guests.Where(x => x.Response == ResponseType.Accepted))
            {
                var postModel = ServiceManager.Kernel.Get<IFacebookManager>().GetPosts(i.Guest, e.StartTime, e.EndTime);
                foreach (var p in postModel.posts)
                {
                    dbPosts.Add(new Post
                    {
                        From = i.Guest,
                        FromId = i.GuestId,
                        Message = p.message,
                        SendTime = p.created_time.DateTime
                    });
                }
            }

            var eventViewModel = new EventModel
            {
                Id = long.Parse(eventId),
                Posts = dbPosts.OrderBy(p => p.SendTime).Reverse().ToList()
            };

            return PartialView("_PostFeed", eventViewModel);
        }
        
		public bool FooCDNAlgorithm(string key)
		{
			try
			{
				if(key.Equals(ConfigurationManager.AppSettings["simplexKey"]))
				{
					FooSimplex simplex = new FooSimplex();
					simplex.RunAlgorithm();
					return true;
				}
			}
			catch(Exception e)
			{
				System.Diagnostics.Debug.Print(e.Message);
			}

			return false;
		}
        
        public ActionResult EventNotFound()
        {
            return View();
        }
    }
}
