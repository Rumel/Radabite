using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Radabite.Backend.Database;

namespace Radabite.Client.WebClient.ViewModels
{
	public class ViewModel
	{
		public List<User> Friends { get; set; }
		public List<Event> Events { get; set; }
	}
}