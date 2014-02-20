using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using Radabite.Backend.Database;
using System.Configuration;

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

            OAuthWebSecurity.RegisterFacebookClient(
                appId: ConfigurationManager.AppSettings["facebookAppId"],
                appSecret: ConfigurationManager.AppSettings["facebookAppSecret"]);

            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
