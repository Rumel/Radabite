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
                        
			foreach (var i in eventRequest.Guests)
            {
				i.Guest = ServiceManager.Kernel.Get<IUserManager>().GetById(i.GuestId);
			}

			foreach (var p in eventRequest.Posts)
			{
				p.From = ServiceManager.Kernel.Get<IUserManager>().GetById(p.FromId);
			}

			foreach (var i in eventRequest.Guests.Where(x => x.Response == ResponseType.Accepted))
			{
                if (i.Guest.FacebookToken != null)
                {
                    var postModel = ServiceManager.Kernel.Get<IFacebookManager>().GetPosts(i.Guest, eventRequest.StartTime, eventRequest.EndTime);
                    foreach (var p in postModel.posts)
                    {
                        if (!(eventRequest.Posts.Where(x => x.ProviderId == p.providerId.ToString()).Count() > 0))
                        {
                            eventRequest.Posts.Add(new Post
                            {
                                Comments = new List<Post>(),
                                From = i.Guest,
                                FromId = i.GuestId,
                                Message = p.message,
                                SendTime = p.created_time.DateTime,
                                ProviderId = p.providerId.ToString()
                            });
                        }
                    }

                    var photoPostModel = ServiceManager.Kernel.Get<IFacebookManager>().GetPhotos(i.Guest, eventRequest.StartTime, eventRequest.EndTime);
                    foreach (var p in (IEnumerable<FacebookPostModel>)photoPostModel.posts)
                    {
                        if (p.fromId == Double.Parse(i.Guest.FacebookUserId) && !(eventRequest.Posts.Where(x => x.ProviderId == p.providerId.ToString()).Count() > 0))
                        {
                            var mime = "image/" + p.photoUrl.Split('.').Last();
                            var blobId = ServiceManager.Kernel.Get<IFooCDNManager>().SaveNewItem(p.photoBytes, mime, eventRequest.StorageLocation);
                            eventRequest.Posts.Add(new Post
                            {
                                Comments = new List<Post>(),
                                From = i.Guest,
                                FromId = i.GuestId,
                                Message = p.message,
                                SendTime = p.created_time.DateTime,
                                BlobId = blobId.Value.ToString(),
                                Mimetype = mime,
                                ProviderId = p.providerId.ToString()
                            });
                        }
                    }
                }

                if (i.Guest.TwitterToken != null)
                {
                    var tweets = ServiceManager.Kernel.Get<ITwitterManager>().GetTweets(i.Guest, eventRequest.StartTime, eventRequest.EndTime);
                    foreach (var p in tweets.posts)
                    {
                        if (!(eventRequest.Posts.Where(x => x.ProviderId == p.providerId.ToString()).Count() > 0))
                        {
                            eventRequest.Posts.Add(new Post
                            {
                                Comments = new List<Post>(),
                                From = i.Guest,
                                FromId = i.GuestId,
                                Message = p.message,
                                SendTime = p.created_time.DateTime,
                                ProviderId = p.providerId.ToString()
                            });
                        }
                    }
                }

            }
            
            ServiceManager.Kernel.Get<IEventManager>().Save(eventRequest);

            var eventViewModel = new EventModel()
            {
                Id = eventRequest.Id,
                Title = eventRequest.Title,
                StartTime = TimeZone.CurrentTimeZone.ToLocalTime(eventRequest.StartTime),
                EndTime = TimeZone.CurrentTimeZone.ToLocalTime(eventRequest.EndTime),
                IsPrivate = eventRequest.IsPrivate,
                Description = eventRequest.Description,
                LocationName = eventRequest.Location.LocationName,
                Latitude = eventRequest.Location.Latitude,
                Longitude = eventRequest.Location.Longitude,
                Posts = eventRequest.Posts.OrderBy(p => p.SendTime).Reverse().ToList(),
                Owner = eventRequest.Owner,
                CurrentUser = currentUser,
				Guests = eventRequest.Guests.ToList(),
				PollIsActive = eventRequest.PollIsActive,
				Votes = eventRequest.Votes.ToList()
            };

            eventViewModel.CurrentUser.Friends = ServiceManager.Kernel.Get<IUserManager>().GetAll().ToList();

            return View(eventViewModel);
        }

        public FileContentResult GetImg(string blobId, string mimetype)
        {
            var response = ServiceManager.Kernel.Get<IFooCDNManager>().Get(blobId, mimetype);
            return new FileContentResult(response.Value as byte[], mimetype);
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

            var events = ServiceManager.Kernel.Get<IEventManager>().GetAll().Where(x => x.IsPrivate == false && x.IsActive).ToList();

			userModel.DiscoverEvents = events.Select(x => new EventModel()
			{
				Id = x.Id,
				Title = x.Title,
				Latitude = x.Location.Latitude,
				Longitude = x.Location.Longitude,
				Distance = Double.NaN,
				StartTime = TimeZone.CurrentTimeZone.ToLocalTime(x.StartTime),
				EndTime = TimeZone.CurrentTimeZone.ToLocalTime(x.EndTime)
			}).ToList();


            userModel.EventInvitations = ServiceManager.Kernel.Get<IEventManager>().GetByGuestId(user.Id);
            
			return View(userModel);
		}

        [HttpPost]
        public RedirectToRouteResult Delete(EventModel model)
        {
            var newEvent = new Event()
            {
                Id = model.Id,
                StartTime = TimeZone.CurrentTimeZone.ToUniversalTime(new DateTime(model.StartTime.Ticks)),
                EndTime = TimeZone.CurrentTimeZone.ToUniversalTime(new DateTime(model.EndTime.Ticks)),
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
                StartTime = TimeZone.CurrentTimeZone.ToUniversalTime(new DateTime(model.StartTime.Ticks)),
                EndTime = TimeZone.CurrentTimeZone.ToUniversalTime(new DateTime(model.EndTime.Ticks)),
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
				Posts = new List<Post>(),
				PollIsActive = model.PollIsActive
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

			ServiceManager.Kernel.Get<IEventManager>().Save(newEvent);

			newEvent.Votes = new List<Vote>()
			{
				new Vote
				{
					Time = TimeZone.CurrentTimeZone.ToUniversalTime(new DateTime(model.StartTime.Ticks)),
					UserName = user.DisplayName
				}
			};

            var result = ServiceManager.Kernel.Get<IEventManager>().Save(newEvent);

            if (model.ToFacebook)
            {
                ServiceManager.Kernel.Get<IFacebookManager>().PublishStatus(user, "Hey guys, I just created an event on Radabite at http://radabite.azurewebsites.net/Event?eventId=" + result.Result.Id + ", make sure to check it out!");
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
			var oldEvent = ServiceManager.Kernel.Get<IEventManager>().GetById(model.Id);

            var newEvent = new Event()
            {
                Id = model.Id,
                StartTime = TimeZone.CurrentTimeZone.ToUniversalTime(new DateTime(model.StartTime.Ticks)),
                EndTime = TimeZone.CurrentTimeZone.ToUniversalTime(new DateTime(model.EndTime.Ticks)),
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
				PollIsActive = model.PollIsActive,
                Owner = oldEvent.Owner,
				Votes = oldEvent.Votes
            };

			newEvent.Votes.Where(x => x.UserName.Equals(newEvent.Owner.DisplayName)).FirstOrDefault().Time = newEvent.StartTime;

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
            if (friends != null)
            {
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
            }

            var eventModel = new EventModel
            {
                CurrentUser = ServiceManager.Kernel.Get<IUserManager>().GetByUserName(User.Identity.Name),
                Guests = e.Guests.ToList()
            };

            return PartialView("_InvitationPanel", eventModel);
        }

        [HttpPost]
        public PartialViewResult JoinEvent(string user, long eventId)
        {
            var e = ServiceManager.Kernel.Get<IEventManager>().GetById(eventId);
            if (user != null)
            {
                e.Guests.Add(new Invitation
                {
                    Guest = ServiceManager.Kernel.Get<IUserManager>().GetById(long.Parse(user)),
                    GuestId = long.Parse(user),
                    Response = ResponseType.Accepted
                });
                ServiceManager.Kernel.Get<IEventManager>().Save(e);
            }

            var eventModel = new EventModel
            {
                CurrentUser = ServiceManager.Kernel.Get<IUserManager>().GetByUserName(User.Identity.Name),
                Guests = e.Guests.ToList()
            };

            return PartialView("_InvitationPanel", eventModel);
        }

        [HttpPost]
        public PartialViewResult RespondToInvitation(string userId, string eventId, string response)
        {
            var u = ServiceManager.Kernel.Get<IUserManager>().GetById(long.Parse(userId));
            var e = ServiceManager.Kernel.Get<IEventManager>().GetById(long.Parse(eventId));
            var r = ResponseType.WaitingReply;
            
            if (response.Equals("Accept"))
            {
                r = ResponseType.Accepted;
            }
            else if (response.Equals("Decline"))
            {
                r = ResponseType.Rejected;
            }

            if (e.Guests.FirstOrDefault(g => g.GuestId == long.Parse(userId)) != null)
            {
                e.Guests.FirstOrDefault(g => g.GuestId == long.Parse(userId)).Response = r;
            }

            ServiceManager.Kernel.Get<IEventManager>().Save(e);

            var userModel = new UserModel
            {
                User = u,
                UserId = u.Id.ToString(),
                EventInvitations = ServiceManager.Kernel.Get<IEventManager>().GetByGuestId(u.Id)
            };

            return PartialView("_DiscoverInvitationList", userModel);
        }

        [HttpPost]
        public PartialViewResult PostFromRadabite(string eventId, string username, string message, bool toFacebook)
        {
            var e = ServiceManager.Kernel.Get<IEventManager>().GetById(long.Parse(eventId));
            var u = ServiceManager.Kernel.Get<IUserManager>().GetByUserName(username);
            var newPost = new Post 
            {
                Comments = new List<Post>(),
                From = u,
                FromId = u.Id,
                Message = message,
                SendTime = TimeZone.CurrentTimeZone.ToUniversalTime(DateTime.Now),
                Likes = 0
            };
            
            e.Posts.Add(newPost);
            
            ServiceManager.Kernel.Get<IEventManager>().Save(e);

            if (toFacebook)
            {
                ServiceManager.Kernel.Get<IFacebookManager>().PublishStatus(u, message);
            }
            
            var eventViewModel = new EventModel
            {
                Id = long.Parse(eventId),
                Posts = e.Posts.OrderBy(p => p.SendTime).Reverse().ToList()
            };

            return PartialView("_PostFeed", eventViewModel);
        }

        [HttpPost]
        public PartialViewResult CommentFromRadabite(string eventId, string postId, string username, string message)
        {
            var e = ServiceManager.Kernel.Get<IEventManager>().GetById(long.Parse(eventId));
            var u = ServiceManager.Kernel.Get<IUserManager>().GetByUserName(username);
            var newComment = new Post
            {
                From = u,
                FromId = u.Id,
                Message = message,
                SendTime = TimeZone.CurrentTimeZone.ToUniversalTime(DateTime.Now),
                Likes = 0
            };

            if (e.Posts.FirstOrDefault(x => x.Id == long.Parse(postId)) != null)
            {
                e.Posts.FirstOrDefault(x => x.Id == long.Parse(postId)).Comments.Add(newComment);
            }

            ServiceManager.Kernel.Get<IEventManager>().Save(e);

            var eventViewModel = new EventModel
            {
                Id = long.Parse(eventId),
                Posts = e.Posts.OrderBy(p => p.SendTime).Reverse().ToList()
            };

            return PartialView("_PostFeed", eventViewModel);
        }

		[HttpPost]
		public PartialViewResult Vote(string eventId, string username, string vote)
		{
			var mEvent = ServiceManager.Kernel.Get<IEventManager>().GetById(long.Parse(eventId));
			var user = ServiceManager.Kernel.Get<IUserManager>().GetByUserName(username);

			DateTime dt;
			try
			{
				dt = TimeZone.CurrentTimeZone.ToUniversalTime(Convert.ToDateTime(vote));
			}
			catch(Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e);
				return PartialView("_VotedError");
			}
			
			//If the user has already voted, change the vote
			if (mEvent.Votes.Select(x => x.UserName).Contains(user.DisplayName))
			{
				mEvent.Votes.Where(x => x.UserName.Equals(user.DisplayName)).FirstOrDefault().Time = dt;
			}
			else
			{
				mEvent.Votes.Add(new Vote() { Time = dt, UserName = user.DisplayName });
			}

			ServiceManager.Kernel.Get<IEventManager>().Save(mEvent);

			EventModel viewModel = new EventModel()
			{
				Id = long.Parse(eventId),
				PollIsActive = mEvent.PollIsActive,
				Votes = mEvent.Votes.ToList<Vote>()
			};

			return PartialView("_VotedPartial", viewModel);
		}

		[HttpPost]
		public PartialViewResult AllEvents(double userLat, double userLong)
		{
			var events = ServiceManager.Kernel.Get<IEventManager>().GetAll().Where(x => x.IsActive);

			List<EventModel> viewModels = events.Select(x => new EventModel() 
			{ 
				Id = x.Id,
				Title = x.Title,
				Latitude = x.Location.Latitude,
				Longitude = x.Location.Longitude,
				StartTime = TimeZone.CurrentTimeZone.ToLocalTime(x.StartTime),
				EndTime = TimeZone.CurrentTimeZone.ToLocalTime(x.EndTime)
			}).ToList();

			foreach(var v in viewModels)
			{
				v.Distance = v.CalcDistance(userLat, userLong);
			}


			return PartialView("_EventList", viewModels);
		}

		[HttpPost]
		public PartialViewResult SortEventsLocation(double userLat, double userLong)
		{
			var events = ServiceManager.Kernel.Get<IEventManager>().GetAll().Where(x => x.IsActive);

			List<EventModel> viewModels = events.Select(x => new EventModel()
			{
				Id = x.Id,
				Title = x.Title,
				Latitude = x.Location.Latitude,
				Longitude = x.Location.Longitude,
				StartTime = TimeZone.CurrentTimeZone.ToLocalTime(x.StartTime),
				EndTime = TimeZone.CurrentTimeZone.ToLocalTime(x.EndTime)
			}).ToList();

			foreach (var v in viewModels)
			{
				v.Distance = v.CalcDistance(userLat, userLong);
			}

			viewModels = viewModels.Where(x => !x.Distance.Equals(Double.NaN)).OrderBy(x => x.Distance).ToList();

			return PartialView("_EventList", viewModels);
		}

		[HttpPost]
		public PartialViewResult SortEventsTime(double userLat, double userLong)
		{
			var events = ServiceManager.Kernel.Get<IEventManager>().GetAll().Where(x => x.IsActive);

			List<EventModel> viewModels = events.Select(x => new EventModel()
			{
				Id = x.Id,
				Title = x.Title,
				Latitude = x.Location.Latitude,
				Longitude = x.Location.Longitude,
				StartTime = TimeZone.CurrentTimeZone.ToLocalTime(x.StartTime),
				EndTime = TimeZone.CurrentTimeZone.ToLocalTime(x.EndTime)
			}).ToList();

			foreach (var v in viewModels)
			{
				v.Distance = v.CalcDistance(userLat, userLong);
			}

			viewModels = viewModels.Where(x => x.EndTime > TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Central Standard Time"))
				.OrderBy(x => (x.StartTime - TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Central Standard Time"))).ToList();

			return PartialView("_EventList", viewModels);
		}

		public bool FooCDNAlgorithm(string key)
		{
			try
			{
				if (key.Equals(ConfigurationManager.AppSettings["simplexKey"]))
				{
					FooSimplex simplex = new FooSimplex();
					simplex.RunAlgorithm();
					return true;
				}
			}
			catch (Exception e)
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
