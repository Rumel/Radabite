using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Radabite.Backend.Database;

namespace Radabite.Client.WebClient.ViewModels
{
	//probably shouldn't be named this in case we want to make other ViewModels
		//but not sure how to name because I don't know exactly how I'm going to use this
	public class ViewModel
	{
		public List<User> Friends { get; set; }
		public List<Event> Events { get; set; }
	}
}