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

		public ActionResult Index(long userId)
        {
            ViewBag.Message = userId.ToString() + "'s profile page.";
            ViewBag.userId = userId;

			var friends = ServiceManager.Kernel.Get<IUserManager>().GetById(userId).Friends;
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
