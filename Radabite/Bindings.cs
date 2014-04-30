using Ninject.Modules;
using Radabite.Backend.Accessors;
using Radabite.Backend.Interfaces;
using Radabite.Backend.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Radabite
{
    public class Bindings : NinjectModule
    {
        public override void Load()
		{
			Bind<IFooCDNManager>().To<FooCDNManager>();
            Bind<IEventManager>().To<EventManager>();
            Bind<IUserManager>().To<UserManager>();
            Bind<IFacebookManager>().To<FacebookManager>();
            Bind<ITwitterManager>().To<TwitterManager>();

			Bind<IFooCDNAccessor>().To<FooCDNAccessor>();
            Bind<IEventAccessor>().To<EventAccessor>();
            Bind<IUserAccessor>().To<UserAccessor>();

        }
    }
}