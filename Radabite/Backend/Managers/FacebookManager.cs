using Facebook;
using Radabite.Backend.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Radabite.Backend.Managers
{
    public class FacebookManager : IFacebookManager
    {

        public void GetPosts(string userId, string accessToken, DateTime startTime, DateTime endTime)
        {
            var fb = new FacebookClient();
            fb.Get("cassey.lottman");
           // var query = string.Format(""); // query
            
        }

        public void PracticeGet()
        {
            var fb = new FacebookClient();
            var result = fb.Get("cassey.lottman");
        }

        // http://facebooksdk.net/docs/datetimeconverter/
        public double ConvertToFacebookTime(DateTime startTime)
        {
            DateTime epoch = DateTimeConvertor.Epoch;
            int pdtOffset = -25200; // pacific daylight time offset in seconds
            DateTimeOffset dtOffset = new DateTimeOffset(startTime, TimeSpan.FromSeconds(pdtOffset));
            double unixTime = DateTimeConvertor.ToUnixTime(dtOffset);
            return unixTime;
        }
        // http://facebooksdk.net/docs/datetimeconverter/
        public DateTime ConvertFromUnixTime(double startTime)
        {
            DateTime result = DateTimeConvertor.FromUnixTime(startTime);
            return result;
        }

        public string GetAccessToken(FacebookClient fb)
        {
            dynamic result = fb.Get("oauth/access_token", new
                {
                    client_id = ConfigurationManager.AppSettings["facebookAppId"],
                    client_secret = ConfigurationManager.AppSettings["facebookAppSecret"],
                    grant_type = "client_credentials"
                });
            return result.access_token;
        }


    }
}