using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Radabite.Backend.Database;
using Radabite.Database;

namespace Radabite.Interfaces
{
    interface IEventAccessor
    {
        SaveResult<Event> Save(Event e);

        IEnumerable<Event> GetAll();
    }
}
