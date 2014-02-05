using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Radabite.Backend.Database
{
    public class User
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string TwitterHandle { get; set; }
        public string FacebookName { get; set; }
        public string Email { get; set; }
        public string LinkToPhoto { get; set; }
        public string SelfDescription { get; set; }
        public int Age { get; set; }

    }
}