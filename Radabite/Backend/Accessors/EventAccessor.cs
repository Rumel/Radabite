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
                if(e.Id != 0)
                {
                    var ev = db.Events.FirstOrDefault(x => x.Id == e.Id);
                    ev.Title = e.Title;
                    ev.StartTime = e.StartTime;
                    ev.EndTime = e.EndTime;
                    ev.IsPrivate = e.IsPrivate;
                    ev.Description = e.Description;
                    ev.Location = e.Location;
                    ev.FinishedGettingPosts = e.FinishedGettingPosts;
                    ev.IsActive = e.IsActive;
                    if (e.Guests != null)
                    {
                        foreach (var i in e.Guests)
                        {
                            i.Guest = db.Users.FirstOrDefault(x => x.Id == i.GuestId);
                            if (i.Id != 0)
                            {
                                db.Entry<Invitation>(i).State = EntityState.Modified;
                            }
                            else
                            {
                                ev.Guests.Add(i);
                                db.Entry<Invitation>(i).State = EntityState.Added;
                            }
                        }
                    }

                    if (e.Posts != null)
                    {
                        foreach (var p in e.Posts)
                        {
                            if (p.Id == 0)
                            {
                                p.From = db.Users.FirstOrDefault(x => x.Id == p.From.Id);
                                ev.Posts.Add(p);
                                db.Entry<Post>(p).State = EntityState.Added;
                                db.Entry<User>(p.From).State = EntityState.Modified;
                            }
                            if (p.Comments != null)
                            {
                                foreach (var c in p.Comments)
                                {
                                    if (c.Id == 0)
                                    {
                                        c.From = db.Users.FirstOrDefault(x => x.Id == c.From.Id);
                                        ev.Posts.FirstOrDefault(x => x.Id == p.Id).Comments.Add(c);
                                        db.Entry<Post>(c).State = EntityState.Added;
                                        db.Entry<User>(c.From).State = EntityState.Modified;
                                    }
                                }
                            }
                        }
                    }

                    db.Entry<Event>(ev).State = EntityState.Modified;
                    db.Entry<User>(ev.Owner).State = EntityState.Unchanged;
                } 
                else 
                {
                    e.Owner = db.Users.FirstOrDefault(x => x.Id == e.Owner.Id);
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
                return db.Events.Include(e => e.Location)
                                .Include(e => e.Owner.Events)
                                .Include(e => e.Guests.Select(i => i.Guest))
                                .Include(e => e.Posts.Select(p => p.Comments))
                                .Include(e => e.Posts.Select(p => p.From))
                                .ToList();
            }
        }

        public Event GetById(long id)
        {
            using (var db = new Db())
            {
                return db.Events.Include(e => e.Location)
                                .Include(e => e.Owner.Events)
                                .Include(e => e.Guests.Select(i => i.Guest))
                                .Include(e => e.Posts.Select(p => p.Comments))
                                .Include(e => e.Posts.Select(p => p.From))
                                .FirstOrDefault(x => x.Id == id && x.IsActive == true);
            }
        }

        public List<Event> GetByOwnerId(long OwnerId)
        {
            using (var db = new Db())
            {
                return db.Events.Include(e => e.Location)
                                .Include(e => e.Owner.Events)
                                .Include(e => e.Guests.Select(i => i.Guest))
                                .Include(e => e.Posts.Select(p => p.Comments))
                                .Include(e => e.Posts.Select(p => p.From))
                                .Where(x => x.Owner.Id == OwnerId && x.IsActive == true)
                                .ToList();
            }
        }

        public List<Event> GetByGuestId(long GuestId)
        {
            using (var db = new Db())
            {
                return db.Events.Include(e => e.Location)
                                .Include(e => e.Owner.Events)
                                .Include(e => e.Guests.Select(i => i.Guest))
                                .Include(e => e.Posts.Select(p => p.Comments))
                                .Include(e => e.Posts.Select(p => p.From))
                                .Where(x => x.Guests.Where(y => y.Guest.Id == GuestId).Count() > 0 && x.IsActive == true)
                                .ToList();
            }
        }
    }
}