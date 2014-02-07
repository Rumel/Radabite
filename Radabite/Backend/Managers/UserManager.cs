using Radabite.Backend.Accessors;
using Radabite.Backend.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;

namespace Radabite.Backend.Managers
{
    public class UserManager : IUserManager
    {
     
        public Database.User GetById(long id)
        {
            return ServiceManager.Kernel.Get<IUserManager>().GetById(id);
        }

        public void Save(Database.User user)
        {
            ServiceManager.Kernel.Get<IUserManager>().Save(user);
        }
    }
}