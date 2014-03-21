using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radabite.Backend.Database;
using Radabite.Backend.Interfaces;
using Radabite.Models;

namespace Radabite.Tests.Mocks.Accessors
{
    public class MockUserAccessor : IUserAccessor
    {
        public SaveResult<User> SaveOrUpdate(User t)
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

        public User GetByUserProfile(int userProfileId)
        {
            return new User { FacebookProfile = new UserProfile{UserId = userProfileId} };
        }

        public UserProfile GetUserProfile(string userName)
        {
            return new UserProfile { UserName = userName };
        }
    }
}
