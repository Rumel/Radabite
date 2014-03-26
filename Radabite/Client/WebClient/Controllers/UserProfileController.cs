using Radabite.Backend.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using RadabiteServiceManager;
using Radabite.Backend.Interfaces;
using Radabite.Client.WebClient.Models;

namespace Radabite.Client.WebClient.Controllers
{
	public class UserProfileController : Controller
	{

		public ActionResult Index(string u)
        {	

             var user = ServiceManager.Kernel.Get<IUserManager>().GetByUserName(u);
             var userModel = new UserModel { User = user };

			//TODO: Add friends to UserProfile, so there will actually be friends in the db
			//var friends = ServiceManager.Kernel.Get<IUserManager>().GetById(userId).Friends;
			//dummy empty list of friends
			var friends = new List<User>();			

            //Get list of events that user is involved in
            List<Event> events = new List<Event>();

            for (int i = 90; i < 94; i++)
            {
                events.Add(new Event());
                events.ElementAt<Event>(i-90).Id = i;
                events.ElementAt<Event>(i-90).Title = "G-Ma's " + (i+9) +"th B-day!";
                events.ElementAt<Event>(i-90).StartTime = DateTime.Now;
                events.ElementAt<Event>(i-90).EndTime = DateTime.Now;
                events.ElementAt<Event>(i-90).Description = "We are going to party " + i + "eva";
            }
            userModel.Events = events;
            userModel.Friends = friends;
            return View(userModel);
        }

	}
}
