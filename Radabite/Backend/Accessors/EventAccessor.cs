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
                    foreach(var g in e.Guests.Where(h => h.Id == 0))
                    {
                        db.Entry(g).State = EntityState.Added;
                    }
                } else {
                    db.Entry(e).State = EntityState.Added;
                    db.Entry(e.Owner).State = EntityState.Unchanged;
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
                                .Include(e => e.Owner)
                                .Include(e => e.Guests)
                                .ToList();
            }
        }


        public Event GetById(long id)
        {
            using (var db = new Db())
            {
                return db.Events.Include(e => e.Location)
                                .Include(e => e.Owner)
                                .Include(e => e.Guests)
                                .FirstOrDefault(x => x.Id == id && x.IsActive == true);
            }
        }

        public List<Event> GetByOwnerId(long OwnerId)
        {
            using (var db = new Db())
            {
                return db.Events.Include(e => e.Location)
                                .Include(e => e.Owner)
                                .Include(e => e.Guests)
                                .Where(x => x.Owner.Id == OwnerId && x.IsActive == true)
                                .ToList();
            }
        }

        public List<Event> GetByGuestId(long GuestId)
        {
            using (var db = new Db())
            {
                return db.Events.Include(e => e.Location)
                                .Include(e => e.Owner)
                                .Include(e => e.Guests)
                                .Where(x => x.Guests.Where(y => y.Guest.Id == GuestId).Count() > 0 && x.IsActive == true)
                                .ToList();
            }
        }
    }
}