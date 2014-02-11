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
        private GenericAccessor<User> User;

        public MockUserAccessor()
        {
            User = new GenericAccessor<User>();    
        }

        public SaveResult<User> Save(User t)
        {
            return User.Save(t);
        }

        public User GetById(long id)
        {
            return User.GetById(id);
        }

        public IEnumerable<User> GetAll()
        {
            return User.GetAll();
        }
    }
}
