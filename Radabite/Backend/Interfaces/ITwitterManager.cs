using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Radabite.Backend.Database;
using Radabite.Client.WebClient.Models;

namespace Radabite.Backend.Interfaces
{
    public interface ITwitterManager
    {
        GetPostsResult GetTweets(User user, DateTime startTime, DateTime endTime);
    }
}