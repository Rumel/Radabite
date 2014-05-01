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
using WebMatrix.WebData;
using Microsoft.Web.WebPages.OAuth;
using DotNetOpenAuth.AspNet;
using System.Web.Security;
using Radabite.Models;

namespace Radabite.Client.WebClient.Controllers
{
	public class UserProfileController : Controller
	{

		public ActionResult Index(int u)
        {
            var user = ServiceManager.Kernel.Get<IUserManager>().GetById(u);
            if (user != null)
            {
                var userModel = new UserModel { User = user };

                var friends = ServiceManager.Kernel.Get<IUserManager>().GetAll().Where(x => x.Id != user.Id).ToList();

                //Get list of events that user is involved in
				var events = ServiceManager.Kernel.Get<IEventManager>().GetByOwnerId(user.Id);
				userModel.DiscoverEvents = events.Select(x => new EventModel()
				{
					Id = x.Id,
					Title = x.Title,
					Latitude = x.Location.Latitude,
					Longitude = x.Location.Longitude,
					Distance = Double.NaN,
                    StartTime = x.StartTime
				}).ToList();
                userModel.Friends = friends;
                return View(userModel);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult CurrentUser()
        {
            /*AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("CurrentUser"));
            SimpleMembershipProvider provider = (SimpleMembershipProvider)Membership.Provider;
            int id = provider.GetUserIdFromOAuth(result.Provider, result.ProviderUserId);
            */
            int id = WebSecurity.GetUserId(User.Identity.Name);
            return RedirectToAction("Index", new { u = id });

        }
	}
}
