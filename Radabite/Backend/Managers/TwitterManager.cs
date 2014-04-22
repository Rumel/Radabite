using Radabite.Backend.Interfaces;
using Radabite.Client.WebClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tweetinvi;

namespace Radabite.Backend.Managers
{
    public class TwitterManager : ITwitterManager
    {

        public List<GetPostsResult> GetTweets(Radabite.Backend.Database.User user, DateTime startTime, DateTime endTime)
        {
            return new List<GetPostsResult>();
        }

    }
}