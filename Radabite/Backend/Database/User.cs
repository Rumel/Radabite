using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Radabite.Backend.Database
{
    public class User : DataObject
    {
        public long UserId { get; set; }
        public string DisplayName { get; set; }

        public ExternalProfile FacebookProfile { get; set; }

        public ExternalProfile TwitterProfile { get; set; }

        public string Email { get; set; }

        public string PhotoLink { get; set; }

        public string SelfDescription { get; set; }

        public int Age { get; set; }

        public virtual IList<User> Friends { get; set; }
    }
}