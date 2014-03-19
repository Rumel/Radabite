using Radabite.Backend.Accessors;
using Radabite.Backend.Database;
using Radabite.Backend.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using RadabiteServiceManager;

namespace Radabite.Backend.Managers
{
    public class UserManager : IUserManager
    {
     
        public User GetById(long id)
        {
            return ServiceManager.Kernel.Get<IUserAccessor>().GetById(id);
        }

        public SaveResult<User> Save(User t)
        {
            return ServiceManager.Kernel.Get<IUserAccessor>().SaveOrUpdate(t);
        }

        public IEnumerable<User> GetAll()
        {
            return ServiceManager.Kernel.Get<IUserAccessor>().GetAll();
        }
    }
}