using Radabite.Backend.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radabite.Backend.Interfaces
{
    interface IUserManager
    {
        User GetById(long id);
        void Save(User user);
    }
}
