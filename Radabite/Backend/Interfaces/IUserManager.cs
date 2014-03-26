using Radabite.Backend.Database;
using Radabite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radabite.Backend.Interfaces
{
    public interface IUserManager : IGenericManager<User>
    {
        User GetByUserName(string userName);
    }
}
