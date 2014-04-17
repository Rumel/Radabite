using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using Radabite.Backend.Database;
using System.Configuration;
using DotNetOpenAuth.FacebookOAuth2;

namespace Radabite
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            OAuthWebSecurity.RegisterTwitterClient(
                consumerKey: ConfigurationManager.AppSettings["twitterConsumerKey"],
                consumerSecret: ConfigurationManager.AppSettings["twitterSecretKey"]);

            //https://github.com/mj1856/DotNetOpenAuth.FacebookOAuth2
            var facebookAppId = ConfigurationManager.AppSettings["facebookAppId"];
            var facebookAppSecret = ConfigurationManager.AppSettings["facebookAppSecret"];
            var client = new FacebookOAuth2Client(facebookAppId, facebookAppSecret, "read_stream", "publish_actions", "publish_stream", "user_photos");
            var extraData = new Dictionary<string, object>();
            //extraData.Add("read_stream", "read_stream");
            //string[] scope = new string[] {"read_stream"};
            OAuthWebSecurity.RegisterClient(client, "Facebook", extraData);

            /*
            OAuthWebSecurity.RegisterFacebookClient(
                appId: ConfigurationManager.AppSettings["facebookAppId"],
                appSecret: ConfigurationManager.AppSettings["facebookAppSecret"]);
            */
            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
