using Radabite.Backend.Database;
using Radabite.Backend.Interfaces;
using RadabiteServiceManager;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Ninject;
using System.Text;

namespace Radabite.Models
{
	public class EventModel
	{
		private const double EARTH_RADIUS = 3959;

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

		public double Distance { get; set; }

		public double CalcDistance(double userLat, double userLong)
		{
			if(Latitude == -1000 || userLat == -1000)
			{
				return Double.NaN;
			}

			double uLatRadians = userLat * Math.PI / 180,
				uLongRadians = userLong * Math.PI / 180,
				lLatRadians = Latitude * Math.PI / 180,
				lLongRadians = Longitude * Math.PI / 180;

			double dLat = lLatRadians - uLatRadians,
				dLong = lLongRadians - uLongRadians;

			double a = Math.Pow(Math.Sin(dLat / 2.0), 2) +
						Math.Cos(uLatRadians) * Math.Cos(lLatRadians) *
						Math.Pow(Math.Sin(dLong / 2), 2);

			double c = 2.0 * Math.Asin(Math.Sqrt(a));

			return c * EARTH_RADIUS;
		}

		public IOrderedEnumerable<KeyValuePair<DateTime, List<string>>> GetVoteDictionary()
		{
			ConcurrentDictionary<DateTime, List<string>> d = new ConcurrentDictionary<DateTime, List<string>>();

			foreach (var vote in Votes)
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

		public DateTime HasVoted(string username)
		{
			var user = ServiceManager.Kernel.Get<IUserManager>().GetByUserName(username);

			if(Votes.Select(x => x.UserName).Contains(user.DisplayName))
			{
				return Votes.Where(x => x.UserName.Equals(user.DisplayName)).Select(x => x.Time).FirstOrDefault();
			}
			else
			{
				return DateTime.MinValue;
			}
		}

	}

}