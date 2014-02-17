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

        public SaveResult<User> Save(User t)
        {
            using (var db = new Db())
            {
                db.Users.Add(t);
                db.SaveChanges();
            }
            return new SaveResult<User>(true, t);
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