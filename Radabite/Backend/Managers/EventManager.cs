﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Radabite.Accessors;
using Radabite.Backend.Database;
using Radabite.Database;
using Radabite.Interfaces;

namespace Radabite.Managers
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