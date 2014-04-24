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

                //TODO: Add friends to UserProfile, so there will actually be friends in the db
                //var friends = ServiceManager.Kernel.Get<IUserManager>().GetById(userId).Friends;
                //dummy empty list of friends
                var friends = ServiceManager.Kernel.Get<IUserManager>().GetAll().Where(x => x.Id != user.Id).ToList();

                //Get list of events that user is involved in
                userModel.DiscoverEvents = ServiceManager.Kernel.Get<IEventManager>().GetByOwnerId(user.Id);
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
