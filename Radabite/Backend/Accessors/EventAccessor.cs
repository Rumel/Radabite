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
        public SaveResult<Event> SaveOrUpdate(Event e)
        {
            using (var db = new Db())
            {
                if(e.Id != 0){
                    var obj = GetById(e.Id);
                    obj.Id = e.Id;
                    obj.Title = e.Title;
                    obj.Description = e.Description;
                    obj.Location = e.Location;
                    obj.StartTime = e.StartTime;
                    obj.EndTime = e.EndTime;
                    obj.IsPrivate = e.IsPrivate;
                    obj.IsActive = e.IsActive;
                    db.Entry(obj).State = System.Data.EntityState.Modified;
                } else {
                    db.Events.Add(e);
                }
                db.SaveChanges();
            }
            return new SaveResult<Event>(true, e);
        }

        public IEnumerable<Event> GetAll()
        {
            using (var db = new Db())
            {
                return db.Events.Include("Location").ToList();
            }
        }


        public Event GetById(long id)
        {
            using (var db = new Db())
            {
                return db.Events.Include("Location").FirstOrDefault(x => x.Id == id && x.IsActive == true);
            }
        }
    }
}