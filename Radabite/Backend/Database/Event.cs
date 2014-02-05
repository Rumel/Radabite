using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Radabite.Database
{
    public class Event
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}