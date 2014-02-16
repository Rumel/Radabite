using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Radabite.Backend.Database
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        public string DisplayName { get; set; }
        public string TwitterHandle { get; set; }
        public string FacebookName { get; set; }
        public long TwitterUserId { get; set; }
        public long FacebookUserId { get; set; }
        public string Email { get; set; }
        public string PhotoLink { get; set; }
        public string SelfDescription { get; set; }
        public int Age { get; set; }
		public IList<User> Friends { get; set; }
    }
}