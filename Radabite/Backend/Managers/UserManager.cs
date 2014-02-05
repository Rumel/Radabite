using Radabite.Backend.Accessors;
using Radabite.Backend.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Radabite.Backend.Managers
{
    public class UserManager : IUserManager
    {
        private UserAccessor _userAccessor
        {
            get
            {
                return new UserAccessor();
            }
        }

        public Database.User GetById(long id)
        {
            return _userAccessor.GetById(id);
        }

        public void Save(Database.User user)
        {
            _userAccessor.Save(user);
        }
    }
}