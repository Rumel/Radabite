using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Radabite.Backend.Database;
using Radabite.Backend.Accessors;
using Radabite.Backend.Interfaces;
using Ninject;

namespace Radabite.Backend.Managers
{
    public class EventManager : IEventManager
    {

        public SaveResult<Event> Save(Event e)
        {
            return ServiceManager.Kernel.Get<IEventManager>().Save(e);
        }

        public IEnumerable<Event> GetAll()
        {
            return ServiceManager.Kernel.Get<IEventManager>().GetAll();
        }
    }
}