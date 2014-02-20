using Radabite.Backend.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using RadabiteServiceManager;
using Radabite.Backend.Interfaces;

namespace Radabite.Client.WebClient.Controllers
{
	public class UserProfileController : Controller
	{

		public ActionResult Index()
        {	
			//TODO: Add friends to UserProfile, so there will actually be friends in the db
			//var friends = ServiceManager.Kernel.Get<IUserManager>().GetById(userId).Friends;
			//dummy empty list of friends
			var friends = new List<User>();			
			ViewBag.Friends = friends;

            //Get list of events that user is involved in
            List<Event> eventList = new List<Event>();

            for (int i = 90; i < 94; i++)
            {
                eventList.Add(new Event());
                eventList.ElementAt<Event>(i-90).Id = i;
                eventList.ElementAt<Event>(i-90).Title = "G-Ma's " + (i+9) +"th B-day!";
                eventList.ElementAt<Event>(i-90).StartTime = DateTime.Now;
                eventList.ElementAt<Event>(i-90).EndTime = DateTime.Now;
                eventList.ElementAt<Event>(i-90).Description = "We are going to party " + i + "eva";
            }

            return View(eventList);
        }

	}
}
