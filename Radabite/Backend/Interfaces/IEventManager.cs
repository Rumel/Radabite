using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radabite.Backend.Database;

namespace Radabite.Backend.Interfaces
{
    public interface IEventManager : IGenericManager<Event>
    {
        List<Event> GetByOwnerId(long userId);

        List<Event> GetByGuestId(long guestId);
    }
}
