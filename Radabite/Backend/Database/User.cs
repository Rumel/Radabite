using Radabite.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Radabite.Backend.Database
{
    public class User : DataObject
    {
        public string DisplayName { get; set; }

        public string UserName { get; set; }

        public UserProfile TwitterProfile { get; set; }

        public UserProfile FacebookProfile { get; set; }

        public string FacebookUserId { get; set; }

        public string FacebookToken { get; set; }

        public string Email { get; set; }

        public string PhotoLink { get; set; }

        public string SelfDescription { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }

        public string FacebookProfileLink { get; set; }

        public virtual ICollection<User> Friends { get; set; }

        public virtual ICollection<Event> Events { get; set; }

        public string Location { get; set; }
    }
}