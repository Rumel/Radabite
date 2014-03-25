﻿using Facebook;
using Newtonsoft.Json.Linq;
using Radabite.Client.WebClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radabite.Backend.Interfaces
{
    interface IFacebookManager
    {
        //IList<FacebookPostModel> GetPosts(string userId, string accessToken, DateTime startTime, DateTime endTime);
        string GetPosts(string userAccessToken, string userId, DateTime startTime, DateTime endTime);
        string GetAccessToken(FacebookClient fb);


        double ConvertToFacebookTime(DateTime startTime);
        DateTime ConvertFromUnixTime(double startTime);

    }
}
