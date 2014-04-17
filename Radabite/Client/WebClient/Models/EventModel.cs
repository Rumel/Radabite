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

        public EventModel()
        {
            IsActive = true;
        }
    }  

}