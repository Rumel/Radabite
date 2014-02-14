using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radabite.Backend.Database;
using Radabite.Backend.Interfaces;
using Radabite.Tests.Mocks.Accessors;

namespace Radabite.Tests.Mocks.Managers
{
    public class MockUserManager : IUserManager
    {
        public User GetById(long id)
        {
            throw new NotImplementedException();
        }

        SaveResult<User> IGenericManager<User>.Save(User t)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
