using Radabite.Backend.Database;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Radabite.Models
{
    public class EventModel
    {
        public long Id { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsPrivate { get; set; }

        public bool ToFacebook { get; set; }

        public string Description { get; set; }

        public string LocationName { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool IsActive { get; set; }

        public User Owner { get; set; }

        public User CurrentUser { get; set; }
        
        public List<Post> Posts { get; set; }

        public List<Invitation> Guests { get; set; }

		public List<Vote> Votes { get; set; }

		public IOrderedEnumerable<KeyValuePair<DateTime, List<string>>> GetVoteDictionary()
		{
			ConcurrentDictionary<DateTime, List<string>> d = new ConcurrentDictionary<DateTime,List<string>>();

			foreach(var vote in Votes)
			{
				d.AddOrUpdate(vote.Time, new List<string> { vote.UserName }, (k, v) => AddToList(v, vote.UserName));
			}

			var kvPairs = d.ToList().OrderByDescending(x => x.Value.Count);

			return kvPairs;
		}

		private List<string> AddToList(List<string> v, string p)
		{
			v.Add(p);
			return v;
		}

		public bool PollIsActive { get; set; }

        public EventModel()
        {
            IsActive = true;
        }
    }  

}