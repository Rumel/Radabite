using Radabite.Backend.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Radabite.Backend.Database
{
    public class Event : DataObject
    {
        public string Title { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsPrivate { get; set; }

        public string Description { get; set; }

        [ForeignKey("LocationId")]
        public Location Location { get; set; }
    }
}