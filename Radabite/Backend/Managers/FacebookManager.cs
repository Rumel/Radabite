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
using Microsoft.CSharp.RuntimeBinder;
using Radabite.Backend.Database;


namespace Radabite.Backend.Managers
{
    public class FacebookManager : IFacebookManager
    {
        // http://facebooksdk.net/docs/making-asynchronous-requests/
        public IList<FacebookPostModel> GetPosts(string userAccessToken, DateTime startTime, DateTime endTime) 
        {
    
            double unixStartTime = ConvertToUnixTimestamp(startTime);
            double unixEndTime = ConvertToUnixTimestamp(endTime);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://graph.facebook.com");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //HTTP GET
                
                StringBuilder sb = new StringBuilder("/me?fields=statuses.fields(message,from,id)&");
                sb.Append(userAccessToken);
                sb.Append("&since=");
                sb.Append(unixStartTime);
                sb.Append("&until=");
                sb.Append(unixEndTime);
                sb.Append("&limit=50");

                var finalResponse = client.GetAsync(sb.ToString()).Result;
                
                var resString = finalResponse.Content.ReadAsStringAsync().Result;
                List<FacebookPostModel> posts = new List<FacebookPostModel>();
                // Creates a dynamic object with properties of the response
                // this is what we'll use to read the responses into FacebookPostModel objects.
                dynamic fdata = Radabite.Backend.Helpers.JsonUtils.JsonObject.GetDynamicJsonObject(resString);
                try
                {
                    if (fdata != null)
                    {
                        dynamic fstatuses = fdata.statuses.data;
                        foreach (dynamic status in fstatuses)
                        {
                            // double unixTime = Convert.ToDouble(status.updated_time);
                            // DateTime aspTime = ConvertFromUnixTimestamp(unixTime);
                            FacebookPostModel post = new FacebookPostModel
                            {
                                message = status.message,
                                created_time = status.updated_time
                            };
                            posts.Add(post);
                        }
                    }
                }
                catch (RuntimeBinderException e)
                {
                    // this exception probably means part of the json response wasn't what we expected.
                    // just keep going.
                }

                return posts;
            }
        }
        /*
         * GET /oauth/access_token?  
                grant_type=fb_exchange_token&           
                client_id={app-id}&
                client_secret={app-secret}&
                fb_exchange_token={short-lived-token} 
         */
        public string GetFacebookLongTermAccessCode(string shortTermAccessToken)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://graph.facebook.com");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //HTTP GET

                StringBuilder sb = new StringBuilder("/oauth/access_token?grant_type=fb_exchange_token&");
                sb.Append("client_id=");
                sb.Append(ConfigurationManager.AppSettings["facebookAppId"]);
                sb.Append("&client_secret=");
                sb.Append(ConfigurationManager.AppSettings["facebookAppSecret"]);
                sb.Append("&fb_exchange_token=");
                sb.Append(shortTermAccessToken);
                var finalResponse = client.GetAsync(sb.ToString()).Result;

                var accessToken = finalResponse.Content.ReadAsStringAsync().Result;
                return accessToken;
            }
        }

        public FacebookPublishResult PublishStatus(User user, string message)
        {
            using (var client = new HttpClient())
            {
                var result = new FacebookPublishResult();

                var accessToken = user.FacebookToken;
                if (accessToken == null || accessToken == "")
                {
                    result.hasErrors = true;
                    result.errorMessage = "No Facebook access token found for user.";
                }
                else { 
                    client.BaseAddress = new Uri("https://graph.facebook.com");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //HTTP GET

                    StringBuilder apiUri = new StringBuilder("/");
                    apiUri.Append(user.FacebookUserId);
                    apiUri.Append("/feed?");
                    apiUri.Append(accessToken);
                    apiUri.Append("&message=");
                    apiUri.Append(message);
                    
                    var finalResponse = client.PostAsync(apiUri.ToString(), new StringContent("")).Result;
                    StreamContent respContent = (StreamContent)finalResponse.Content;
                    var respContentString = respContent.ReadAsStringAsync().Result;
                    dynamic jsonResp = Radabite.Backend.Helpers.JsonUtils.JsonObject.GetDynamicJsonObject(respContentString);

                    if (finalResponse.StatusCode == HttpStatusCode.OK && jsonResp.id != null)
                    {
                        result.hasErrors = false;
                    }
                    else
                    {
                        result.hasErrors = true;
                        result.errorMessage = jsonResp.errors.message;
                    }

                }
                return result;
            }
        }

        private string HttpPost(string url, string postData)
        {
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(postData);
            string responseData = "";
            using (var responseStream = webRequest.GetResponse().GetResponseStream())
            {
                using (var responseReader = new StreamReader(responseStream))
                {
                    responseData = responseReader.ReadToEnd();
                    return responseData;
                }
            }
        }

        public DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }


        public double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
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