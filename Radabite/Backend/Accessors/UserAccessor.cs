using Radabite.Backend.Database;
using Radabite.Backend.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Radabite.Models;

namespace Radabite.Backend.Accessors
{
    public class UserAccessor : IUserAccessor
    {

        public User GetById(long id)
        {
            using (var db = new Db())
            {
                return db.Users.FirstOrDefault(x => x.Id == id);
            } 
        }

        public SaveResult<User> SaveOrUpdate(User t)
        {
            using (var db = new Db())
            {
                if (t.Id != 0)
                {
                    var obj = GetById(t.Id);
                    obj.Id = t.Id;
                    obj.Age = t.Age;
                    obj.DisplayName = t.DisplayName;
                    obj.Email = t.Email;
                    obj.FacebookProfile = t.FacebookProfile;
                    obj.FacebookProfileLink = t.FacebookProfileLink;
                    obj.Friends = t.Friends;
                    obj.Gender = t.Gender;
                    obj.PhotoLink = t.PhotoLink;
                    obj.SelfDescription = t.SelfDescription;
                    obj.TwitterProfile = t.TwitterProfile;
                    db.Entry(obj).State = System.Data.EntityState.Modified;
                }
                else
                {
                    db.Users.Add(t);
                }
                db.SaveChanges();
                return new SaveResult<User>(true, t);
            }
        }

        public IEnumerable<User> GetAll()
        {
            using (var db = new Db())
            {
                return db.Users.ToList();
            }
        }
    }
}