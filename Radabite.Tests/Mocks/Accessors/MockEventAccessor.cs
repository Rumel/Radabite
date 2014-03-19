using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radabite.Backend.Database;
using Radabite.Backend.Interfaces;

namespace Radabite.Tests.Mocks.Accessors
{
    public class MockEventAccessor : IEventAccessor
    {
        public SaveResult<Event> SaveOrUpdate(Event t)
        {
            t.Id = 1;
            return new SaveResult<Event>(true, t);
        }

        public Event GetById(long id)
        {
            return new Event()
            {
                Id = id
            };
        }

        public IEnumerable<Event> GetAll()
        {
            return new List<Event>
            {
                new Event(),
                new Event(),
                new Event(),
                new Event(),
                new Event()
            };
        }
    }
}
