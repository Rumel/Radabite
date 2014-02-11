using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TweetSharp;

namespace Radabite.Client.WebClient.Controllers
{
    public class SocialMediaController : Controller
    {
        //
        // GET: /SocialMedia/

        public ActionResult Authorize()
        {
            // Step 1 - Retrieve an OAuth Request Token
            TwitterService service = new TwitterService("consumerKey", "consumerSecret");

            // This is the registered callback URL
            OAuthRequestToken requestToken = service.GetRequestToken("http://localhost:8080/SocialMediaController/AuthorizeTwitterCallback");

            // Step 2 - Redirect to the OAuth Authorization URL
            Uri uri = service.GetAuthorizationUri(requestToken);
            return new RedirectResult(uri.ToString(), false /*permanent*/);
        }

        // This URL is registered as the application's callback at http://dev.twitter.com
        public ActionResult AuthorizeTwitterCallback(string oauth_token, string oauth_verifier)
        {
            string _consumerKey = ConfigurationManager.AppSettings["twitterConsumerKey"];
            string _consumerSecret = ConfigurationManager.AppSettings["twitterSecretKey"];        

            var requestToken = new OAuthRequestToken { Token = oauth_token };

            // Step 3 - Exchange the Request Token for an Access Token
            TwitterService service = new TwitterService(_consumerKey, _consumerSecret);
            OAuthAccessToken accessToken = service.GetAccessToken(requestToken, oauth_verifier);

            // Step 4 - User authenticates using the Access Token
            service.AuthenticateWith(accessToken.Token, accessToken.TokenSecret);
            TwitterUser user = service.VerifyCredentials(new VerifyCredentialsOptions());
            //ViewModel.Message = string.Format("Your username is {0}", user.ScreenName);
            return View();
        }

    }
}
