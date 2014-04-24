using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Radabite.Client.WebClient.Models
{
    public class TweetModel : PostModel
    {
        public int retweetsCount { get; set; }
        public int favoritesCount { get; set; }
    }
}