using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Radabite.Backend.Database;
using Radabite.Backend.Interfaces;
using Ninject;
using RadabiteServiceManager;

namespace Radabite.Backend.Managers
{
    public class EventManager : IEventManager
    {

        public SaveResult<Event> Save(Event e)
        {
            return ServiceManager.Kernel.Get<IEventAccessor>().SaveOrUpdate(e);
        }

        public IEnumerable<Event> GetAll()
        {
            return ServiceManager.Kernel.Get<IEventAccessor>().GetAll();
        }


        public Event GetById(long id)
        {
            return ServiceManager.Kernel.Get<IEventAccessor>().GetById(id);
        }
    }
}