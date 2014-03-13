using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Radabite.Models;

namespace Radabite.Backend.Database
{
    public class Db : DbContext
    {

        public DbSet<Event> Events { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public Db()
            : base("DefaultConnection")
        {

        }
    }
}