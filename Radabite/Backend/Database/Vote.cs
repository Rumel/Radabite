using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Radabite.Backend.Database
{
	public class Vote : DataObject
	{
		public string UserName { get; set; }

		public DateTime Time { get; set; }
	}
}