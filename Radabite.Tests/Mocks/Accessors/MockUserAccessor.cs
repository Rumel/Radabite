using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radabite.Backend.Database;
using Radabite.Backend.Interfaces;

namespace Radabite.Tests.Mocks.Accessors
{
    public class MockUserAccessor : IUserAccessor
    {
        public SaveResult<User> Save(User t)
        {
            t.Id = 1;
            return new SaveResult<User>(true, t);
        }

        public User GetById(long id)
        {
            return new User
            {
                Id = 1
            };
        }

        public IEnumerable<User> GetAll()
        {
            return new List<User>
            {
                new User(),
                new User(),
                new User(),
                new User(),
                new User()
            };
        }
    }
}
