using Radabite.Backend.Database;
using Radabite.Backend.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Radabite.Models;
using System.Data.Entity;

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

        public User GetByUserProfile(int p)
        {
            using (var db = new Db())
            {
                return db.Users.FirstOrDefault(x => x.TwitterProfile.UserId == p || x.FacebookProfile.UserId == p);
            }
        }

        public UserProfile GetUserProfile(string userName)
        {
            using (var db = new Db())
            { 
                return db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == userName.ToLower());
            }
        }

        public SaveResult<User> SaveOrUpdate(User u)
        {
            using (var db = new Db())
            {
                if (u.Id != 0)
                {
                    db.Entry(u).State = EntityState.Modified;
                }
                else
                {
                    db.Users.Add(u);
                }

                db.SaveChanges();
                
                return new SaveResult<User>(true, u);
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