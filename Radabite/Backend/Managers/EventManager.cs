using System;
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
            return ServiceManager.Kernel.Get<IEventAccessor>().Save(e);
        }

        public IEnumerable<Event> GetAll()
        {
            return ServiceManager.Kernel.Get<IEventAccessor>().GetAll();
        }


        public Event GetById(long id)
        {
            return ServiceManager.Kernel.Get<IEventAccessor>().GetById(id);
        }

        // http://facebooksdk.net/docs/datetimeconverter/
        public double  ConvertToFacebookTime(DateTime startTime) 
        {
            DateTime epoch = DateTimeConvertor.Epoch;
            int pdtOffset = -25200; // pacific daylight time offset in seconds
            DateTimeOffset dtOffset = new DateTimeOffset(startTime, TimeSpan.FromSeconds(pdtOffset));
            double unixTime = DateTimeConvertor.ToUnixTime(dtOffset);
            return unixTime;
        }
        // http://facebooksdk.net/docs/datetimeconverter/
        public DateTime ConvertFromUnixTime(double startTime)
        {
            DateTime result = DateTimeConvertor.FromUnixTime(startTime);
            return result;
        }
    }
}