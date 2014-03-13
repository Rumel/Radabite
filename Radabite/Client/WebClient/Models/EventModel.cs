using Radabite.Backend.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Radabite.Models
{
    public class EventModel
    {
        [Required]
        public string Title { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsPrivate { get; set; }

        public string Description { get; set; }

        public string LocationName { get; set; }

        public double XCoordinate { get; set; }

        public double YCoordinate { get; set; }

        public bool IsActive { get; set; }

        public EventModel()
        {
            IsActive = true;
        }
    }  

}