using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radabite.Backend.Database;

namespace Radabite.Backend.Interfaces
{
    interface IEventManager
    {
        void Save(Event e);

        IEnumerable<Event> GetAll();
    }
}
