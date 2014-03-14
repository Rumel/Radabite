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

namespace Radabite.Backend.Managers
{
    public class FacebookManager : IFacebookManager
    {
        // http://facebooksdk.net/docs/making-asynchronous-requests/
        public async Task<IList<FacebookPostModel>> GetPostsAsync(string userId, string accessToken, DateTime startTime, DateTime endTime)
        {
   //         var fb = new FacebookClient(accessToken);
   //         fb.Get("cassey.lottman");
            double unixStartTime = ConvertToFacebookTime(startTime);
            double unixEndTime = ConvertToFacebookTime(endTime);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://graph.facebook.com");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //HTTP GET
                
                /*
                client.GetAsync("me").ContinueWith(response =>
                {                    
                    response.Result.Content.ReadAsAsync().ContinueWith(json =>
                    {
                        dynamic result = json.Result;
                        string firstName = result.first_name;
                        string lastName = result.last_name;
                    });
                });
                     */

                // what I was doing originally
                //HttpResponseMessage response = await client.GetAsync("me");
                //return JObject.Parse(response.Content.ToString());

                /* Trying really hard but I can't get this to work.
                //http://stackoverflow.com/questions/9799771/whats-wrong-with-this-async-task-method
                var finalResponse = await client.GetAsync("me").ContinueWith(request =>
                    {
                        // the continueWith function wraps the response in a Task, so need to get its result
                        var response = request.Result;
                        return response.Content.ReadAsStringAsync().ContinueWith(t =>
                            {
                                var result = new HttpResponseMessage();
                                response.CreateContent(t.Result);
                                return response;
                            });
                    }).Unwrap();
                */
                var finalResponse = await client.GetAsync("me").ContinueWith(request =>
                    {
                        var read = request.Result.Content.ReadAsStringAsync();
                        return read.Result;
                    });
                //var result = await task.Content.ReadAsStringAsync();
                var facebookPostList = JsonConvert.DeserializeObject<IList<FacebookPostModel>>(finalResponse);
               // var facebookPostList = new List<FacebookPostModel>();
                return facebookPostList;
/*
 * public async Task<CustomerMVC> GetCustomer()
{
    //return control to caller until GetAsync has completed
    var task = await client.GetAsync("api/values");
    //return control to caller until ReadAsStringAsync has completed
    var result = await task.Content.ReadAsStringAsync()
    return JsonConvert.DeserializeObject<CustomerMVC>(result);
}
 */

            }

           // var query = string.Format(""); // query
         //   using (var client = new HttpClient()) { 
        //    var result = await GetPostsAsync();
        //    }
        }

        // http://www.asp.net/web-api/overview/web-api-clients/calling-a-web-api-from-a-net-client
        /*
        public async Task<JObject> GetPostsAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://graph.facebook.com");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //HTTP GET
                HttpResponseMessage response = await client.GetAsync("me");
                return JObject.Parse(response.Content.ToString());
            }
        }
        */

        public void PracticeGet()
        {
            //var fb = new FacebookClient();
            // var result = fb.Get("cassey.lottman");
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