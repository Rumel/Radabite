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
            userModel.Events = ServiceManager.Kernel.Get<IEventManager>().GetByOwnerId(user.Id);
            userModel.Friends = friends;
            return View(userModel);
        }

	}
}
