using System.Reflection;
using Ninject.Modules;
using Radabite.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using RadabiteServiceManager;
using System.Data.Entity;
using Radabite.Backend.Database;

namespace Radabite
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();

            ExtendedRazor engine = new ExtendedRazor();
            engine.AddViewLocationFormat("~/Client/WebClient/Views/{1}/{0}.cshtml");
            engine.AddViewLocationFormat("~/Client/WebClient/Views/{1}/{0}.cshtml");
            engine.AddViewLocationFormat("~/Client/WebClient/Views/{0}.cshtml");
            engine.AddViewLocationFormat("~/Client/WebClient/Views/Shared/{1}/{0}.cshtml");
            // Add a shared location too, as the lines above are controller specific
            engine.AddPartialViewLocationFormat("~/Client/WebClient/Views/{0}.cshtml");
            engine.AddPartialViewLocationFormat("~/Client/WebClient/Views/{0}.vbhtml");
            engine.AddPartialViewLocationFormat("~/Client/WebClient/Views/{1}/{0}.cshtml");
            ViewEngines.Engines.Add(engine);

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<Db>());

            ServiceManager.Kernel.Load(new List<NinjectModule>
            {
                new Bindings()
            });
        }
    }
}