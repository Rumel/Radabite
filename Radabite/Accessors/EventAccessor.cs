using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using Radabite.Database;
using Radabite.Interfaces;

namespace Radabite.Accessors
{
    public class EventAccessor : IEventAccessor
    {
        public void Save(Event e)
        {
            using (var db = new Db())
            {
                db.Events.Add(e);
                db.SaveChanges();
            }           
        }

        public IEnumerable<Event> GetAll()
        {
            using (var db = new Db())
            {
                return db.Events.ToList();
            }
        }
    }
}