using Radabite.Backend.Database;
using Radabite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radabite.Backend.Interfaces
{
    public interface IUserAccessor : IGenericAccessor<User>
    {
        User GetByUserProfile(int userProfileId);
        UserProfile GetUserProfile(string userName);
    }
}
