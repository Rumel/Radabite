using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radabite.Backend.Database;
using Radabite.Backend.Interfaces;

namespace Radabite.Tests.Mocks.Managers
{
    public class MockEventManager : IEventManager
    {
        public SaveResult<Event> Save(Event t)
        {
            throw new NotImplementedException();
        }

        public Event GetById(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Event> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<Event> GetByOwnerId(long ownerId)
        {
            throw new NotImplementedException();
        }

        public List<Event> GetByGuestId(long guestId)
        {
            throw new NotImplementedException();
        }
    }
}
