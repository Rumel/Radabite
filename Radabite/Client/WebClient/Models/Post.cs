using Radabite.Backend.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Radabite.Models
{
    public class Post
    {
        public long EventId { get; set; }

        public User From { get; set; }

        public String Message { get; set; }

        public int Likes { get; set; }

        public DateTime SendTime { get; set; }

    }
}