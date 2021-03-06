﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Radabite.Backend.Database;
using Radabite.Backend.Interfaces;
using Ninject;
using RadabiteServiceManager;
using Facebook;

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
            var all = ServiceManager.Kernel.Get<IEventAccessor>().GetAll();
            return all;
        }

        public Event GetById(long id)
        {
            return ServiceManager.Kernel.Get<IEventAccessor>().GetById(id);
        }

        public List<Event> GetByOwnerId(long ownerId)
        {
            return ServiceManager.Kernel.Get<IEventAccessor>().GetByOwnerId(ownerId);
        }

        public List<Event> GetByGuestId(long guestId)
        {
            return ServiceManager.Kernel.Get<IEventAccessor>().GetByGuestId(guestId);
        }

    }
}