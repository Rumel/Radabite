using Radabite.Backend.Database;
using Radabite.Backend.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Radabite.Backend.Accessors
{
    public class UserAccessor : IUserAccessor
    {

        public User GetById(long id)
        {
            using (var db = new Db())
            {
                return db.Users.Find(id);
            } 
        }

        public void Save(Database.User user)
        {
            using (var db = new Db())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
        }

    }
}