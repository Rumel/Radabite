using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using Radabite.Backend.Database;
using Radabite.Backend.Interfaces;
using System.Data.Entity;


namespace Radabite.Backend.Accessors
{
    public class EventAccessor : IEventAccessor
    {
        public SaveResult<Event> SaveOrUpdate(Event e)
        {
            using (var db = new Db())
            {
                
                if(e.Id != 0){
                    db.Entry(e).State = EntityState.Modified;
                } else {
                    db.Entry(e).State = EntityState.Added;
                }
                db.SaveChanges();
            }
            return new SaveResult<Event>(true, e);
        }

        public IEnumerable<Event> GetAll()
        {
            using (var db = new Db())
            {
                return db.Events.Include(e => e.Location)
                                .Include(e => e.Owner).ToList();
            }
        }


        public Event GetById(long id)
        {
            using (var db = new Db())
            {
                return db.Events.Include(e => e.Location)
                                .Include(e => e.Owner)
                                .FirstOrDefault(x => x.Id == id && x.IsActive == true);
            }
        }
    }
}