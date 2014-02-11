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
        private GenericAccessor<Event> Event;

        public MockEventAccessor()
        {
            Event = new GenericAccessor<Event>();
        }

        public SaveResult<Event> Save(Event t)
        {
            return Event.Save(t);
        }

        public Event GetById(long id)
        {
            return Event.GetById(id);
        }

        public IEnumerable<Event> GetAll()
        {
            return Event.GetAll();
        }
    }
}
