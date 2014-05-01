using Newtonsoft.Json;
using Radabite.Backend.Database;
using Radabite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Radabite.Client.WebClient.Models
{
    public class UserModel
    {
        public User User { get; set; }
        public string AccessToken { get; set; }
        public string UserId { get; set; }
        public List<EventModel> DiscoverEvents { get; set; }
        public List<Event> EventInvitations { get; set; }
        public List<User> Friends { get; set; }

        public string ToCalendarJSon()
        {
            var events = new List<JsonEvent>();
            foreach (var e in DiscoverEvents)
            {
                events.Add(new JsonEvent
                {
                    date = ((e.StartTime.Ticks - new DateTime(1970, 1, 1, 0, 0, 0, 0).Ticks)/10000).ToString(),
                    type = "meeting",
                    title = e.Title,
                    description = (e.Description == null ? "" : e.Description),
                    url = "../Event?eventId=" + e.Id
                });
            }

            return JsonConvert.SerializeObject(events);
        }

        internal class JsonEvent
        {
            public string date;

            public string type;

            public string title;

            public string description;

            public string url;
        }
    }


}