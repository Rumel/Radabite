using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Radabite.Backend.Database;

namespace Radabite.Backend.Interfaces
{
    public interface IEventAccessor : IGenericAccessor<Event>
    {
        List<Event> GetByOwnerId(long ownerId);

    }
}
