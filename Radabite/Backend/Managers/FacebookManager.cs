﻿using Facebook;
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
        const string badUserToken = "No Facebook access token found for user.";

        // http://facebooksdk.net/docs/making-asynchronous-requests/
        public GetPostsResult GetPosts(User user, DateTime startTime, DateTime endTime) 
        {
    
            double unixStartTime = ConvertToUnixTimestamp(startTime);
            double unixEndTime = ConvertToUnixTimestamp(endTime);

            var result = new GetPostsResult();
            if (!hasFacebookToken(user))
            {
                result.hasErrors = true;
                result.errorMessage = badUserToken;
            }
            else {
                var userAccessToken = user.FacebookToken;
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
                                DateTimeOffset time = status.updated_time;
                                if (time >= startTime && time < endTime)
                                {
                                    FacebookPostModel post = new FacebookPostModel
                                    {
                                       // providerId = status.id,
                                        message = status.message,
                                        created_time = status.updated_time
                                    };
                                    if (status.id != null)
                                    {
                                        post.providerId = status.id;
                                    }
                                    posts.Add(post);
                                }
                            }
                        }
                    }
                    catch (RuntimeBinderException e)
                    {
                        // this exception probably means part of the json response wasn't what we expected.
                        // just keep going.
                    }
                    result.posts = posts;
                }
            }
            return result;
        }

        public GetPostsResult GetPhotos(User user, DateTime startTime, DateTime endTime)
        {
            double unixStartTime = ConvertToUnixTimestamp(startTime);
            double unixEndTime = ConvertToUnixTimestamp(endTime);

            var result = new GetPostsResult();
            if (!hasFacebookToken(user))
            {
                result.hasErrors = true;
                result.errorMessage = badUserToken;
            }
            else
            {
                var userAccessToken = user.FacebookToken;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://graph.facebook.com");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //HTTP GET

                    StringBuilder sb = new StringBuilder("/");
                    sb.Append(user.FacebookUserId);
                    sb.Append("/photos?");

                    sb.Append("since=");
                    //sb.Append(unixStartTime);
                    sb.Append(startTime.ToString());
                    sb.Append("&until=");
                    //sb.Append(unixEndTime);
                    sb.Append(endTime.ToString());
                   // sb.Append("&from=");
                   // sb.Append(user.FacebookUserId);

                    sb.Append("&");
                    
                    sb.Append(userAccessToken);                    
                    var finalResponse = client.GetAsync(sb.ToString()).Result;

                    var resString = finalResponse.Content.ReadAsStringAsync().Result;
                    List<FacebookPostModel> posts = new List<FacebookPostModel>();

                    dynamic fdata = Radabite.Backend.Helpers.JsonUtils.JsonObject.GetDynamicJsonObject(resString);
                    try
                    {
                        if (fdata != null)
                        {
                            dynamic fphotos = fdata.data;
                            foreach (dynamic photo in fphotos)
                            {
                                dynamic from = photo.from;
                                FacebookPostModel post = new FacebookPostModel
                                {
                                    providerId = photo.id,
                                    photoUrl = photo.source,
                                };

                                if (from != null)
                                {
                                    post.fromId = from.id;
                                    post.fromName = from.name;
                                }
                                if (photo.name != null)
                                {
                                    post.message = photo.name;
                                }
                                if (photo.created_time != null)
                                {
                                    post.created_time = photo.updated_time;
                                }
                                if (post.fromId == Convert.ToDouble(user.FacebookUserId)) { 
                                    post.photoBytes = GetPhotoBytes(post.photoUrl);
                                    posts.Add(post);
                                }
                            }
                        }
                    }
                    catch (RuntimeBinderException e)
                    {
                        // this exception probably means part of the json response wasn't what we expected.
                        // just keep going.
                    }
                    result.posts = posts;



                }
            }
            return result;
        }
        public byte[] GetPhotoBytes(string url)
        {
            byte[] image;
            using (var webClient = new WebClient())
            {
                image = webClient.DownloadData(url);
                return image;
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


        public string GetProfilePictureUrl(User user) 
        {
            using (var client = new HttpClient())
            {
                var result = new FacebookPublishResult();
                string url = "";
                if (!hasFacebookToken(user))
                {
                    result.hasErrors = true;
                    result.errorMessage = badUserToken;
                }
                else
                {
                    var accessToken = user.FacebookToken;
                    client.BaseAddress = new Uri("https://graph.facebook.com");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //HTTP GET

                    StringBuilder apiUri = new StringBuilder("/");
                    apiUri.Append(user.FacebookUserId);
                    // redirect false to get the url as json instead of a redirect to the picture
                    // for size, can also do type=large instead of hard-coding height and width
                    apiUri.Append("/picture?height=300&width=300&redirect=false");
                    var response = client.GetAsync(apiUri.ToString()).Result;
                    var resString = response.Content.ReadAsStringAsync().Result;
                    dynamic resJson = Radabite.Backend.Helpers.JsonUtils.JsonObject.GetDynamicJsonObject(resString);
                    try {
                        url = resJson.data.url;
                    }
                    catch (RuntimeBinderException e) {
                        /* json wasn't as expected. carry on. */
                        /* if we cared more we would log this and fix the json parse */
                    }
                }
                return url;
            }
            
        }


        public FacebookPublishResult PublishStatus(User user, string message)
        {
            using (var client = new HttpClient())
            {
                var result = new FacebookPublishResult();

                if (!hasFacebookToken(user))
                {
                    result.hasErrors = true;
                    result.errorMessage = badUserToken;
                }
                else {
                    var accessToken = user.FacebookToken;
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

        private bool hasFacebookToken(User user)
        {
            var userAccessToken = user.FacebookToken;
            bool hasToken = true;
            if (userAccessToken == null || userAccessToken == "")
            {
                hasToken = false;
            }
            return hasToken;
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