using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using Radabite.Backend.Database;
using Radabite.Backend.Interfaces;


namespace Radabite.Backend.Accessors
{
    public class EventAccessor : IEventAccessor
    {
        public SaveResult<Event> Save(Event e)
        {
            using (var db = new Db())
            {
                db.Events.Add(e);
                db.SaveChanges();
            }
            return new SaveResult<Event>(true, e);
        }

        public IEnumerable<Event> GetAll()
        {
            using (var db = new Db())
            {
                return db.Events.ToList();
            }
        }


        public Event GetById(long id)
        {
            using (var db = new Db())
            {
                return db.Events.FirstOrDefault(x => x.Id == id);
            }
        }
    }
}