using Facebook;
using Radabite.Backend.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Radabite.Client.WebClient.Models;
using System.Text;
using System.Net;
using System.IO;
using Microsoft.Web.WebPages.OAuth;
using DotNetOpenAuth.AspNet;


namespace Radabite.Backend.Managers
{
    public class FacebookManager : IFacebookManager
    {
        // http://facebooksdk.net/docs/making-asynchronous-requests/
        public string GetPosts(string userAccessToken, string userId, string accessToken, DateTime startTime, DateTime endTime) 
        {

            // if we were using the facebook sdk we would do this.
        //  var fb = new FacebookClient(accessToken);
        //  fb.Get("cassey.lottman");
    
            double unixStartTime = ConvertToFacebookTime(startTime);
            double unixEndTime = ConvertToFacebookTime(endTime);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://graph.facebook.com");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //HTTP GET
                
                string facebookAccessToken = userAccessToken;
                StringBuilder sb = new StringBuilder("/me?fields=statuses&");
                sb.Append("access_token=");
                sb.Append(facebookAccessToken);
             
                var finalResponse = client.GetAsync(sb.ToString()).Result;
                
                var resString = finalResponse.Content.ReadAsStringAsync().Result;

                // Creates a dynamic object with properties of the response
                // this is what we'll use to read the responses into FacebookPostModel objects.
                dynamic fstatuses = Radabite.Backend.Helpers.JsonUtils.JsonObject.GetDynamicJsonObject(resString);

                // this is another way of doing it but the json responses are so nested that this will be very messy. 
                // dynamic's the way to go.
            //    FacebookPageResults response = (FacebookPageResults) JsonConvert.DeserializeObject(resString);

                return finalResponse.Content.ReadAsStringAsync().Result;
            }
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