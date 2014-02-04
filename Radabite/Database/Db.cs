using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Radabite.Database
{
    public class Db : DbContext
    {
        public DbSet<Event> Events { get; set; }
    }
}