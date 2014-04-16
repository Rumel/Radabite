using Radabite.Backend.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Radabite.Client.WebClient.Models
{
    public class UserModel
    {
        public User User { get; set; }
        public string AccessToken { get; set; }
        public string UserId { get; set; }
        public List<Event> DiscoverEvents { get; set; }
        public List<Event> EventInvitations { get; set; }
        public List<User> Friends { get; set; }
    }
}