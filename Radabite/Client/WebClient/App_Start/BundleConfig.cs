using System.Web;
using System.Web.Optimization;

namespace Radabite
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Client/WebClient/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Client/WebClient/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Client/WebClient/Scripts/jquery.unobtrusive*",
                        "~/Client/WebClient/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Client/WebClient/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Client/WebClient/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Client/WebClient/Content/themes/base/css").Include(
                        "~/Client/WebClient/Content/themes/base/jquery.ui.core.css",
                        "~/Client/WebClient/Content/themes/base/jquery.ui.resizable.css",
                        "~/Client/WebClient/Content/themes/base/jquery.ui.selectable.css",
                        "~/Client/WebClient/Content/themes/base/jquery.ui.accordion.css",
                        "~/Client/WebClient/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Client/WebClient/Content/themes/base/jquery.ui.button.css",
                        "~/Client/WebClient/Content/themes/base/jquery.ui.dialog.css",
                        "~/Client/WebClient/Content/themes/base/jquery.ui.slider.css",
                        "~/Client/WebClient/Content/themes/base/jquery.ui.tabs.css",
                        "~/Client/WebClient/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Client/WebClient/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Client/WebClient/Content/themes/base/jquery.ui.theme.css"));

            #region Foundation Bundles
                bundles.Add(new StyleBundle("~/Content/foundation/css").Include(
                    "~/Client/WebClient/Content/foundation/foundation.css",
                    "~/Client/WebClient/Content/foundation/foundation.mvc.css",
                    "~/Client/WebClient/Content/foundation/app.css"));

                bundles.Add(new ScriptBundle("~/bundles/foundation").Include(
                    "~/Client/WebClient/Scripts/foundation/foundation.js",
                    "~/Client/WebClient/Scripts/foundation/foundation.*",
                    "~/Client/WebClient/Scripts/foundation/app.js"));
            #endregion

                bundles.Add(new StyleBundle("~/Content/Radabite").Include(
                    "~/Client/WebClient/Content/Radabite.css",
                    "~/Client/WebClient/Content/themes/base/zocial.css"));
        }
    }
}