﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Radabite.Backend.Database;
using Radabite.Backend.Accessors;
using Radabite.Backend.Interfaces;

namespace Radabite.Backend.Managers
{
    public class EventManager : IEventManager
    {
        private EventAccessor _eventAccessor
        {
            get
            {
                return new EventAccessor();
            }
        }

        public SaveResult<Event> Save(Event e)
        {
            return _eventAccessor.Save(e);
        }

        public IEnumerable<Event> GetAll()
        {
            return _eventAccessor.GetAll();
        }
    }
}