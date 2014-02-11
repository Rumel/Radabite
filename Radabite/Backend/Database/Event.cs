﻿using Radabite.Backend.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Radabite.Backend.Database
{
    public class Event
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsPrivate { get; set; }

        public string Description { get; set; }

        public Location Location { get; set; }
    }
}