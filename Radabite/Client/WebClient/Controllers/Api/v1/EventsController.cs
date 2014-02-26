using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Radabite.Backend.Database;
using Radabite.Backend.Managers;
using Ninject;
using Radabite.Backend.Interfaces;
using RadabiteServiceManager;

namespace Radabite.Controllers.Api
{
    public class EventsController : ApiController
    {
      

        // GET api/<controller>
        public IEnumerable<Event> Get()
        {
            return ServiceManager.Kernel.Get<IEventManager>().GetAll();
        }

        // GET api/<controller>/5
        public Event Get(int id)
        {
            return ServiceManager.Kernel.Get<IEventManager>().GetById(id);
        }

        // POST api/<controller>
        public Event Post([FromBody]string value)
        {
            var result = ServiceManager.Kernel.Get<IEventManager>().Save(new Event()
            {
                Description = "My cool description",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now + TimeSpan.FromHours(2),
                IsPrivate = false,
                Location = new Location()
                {
                    Latitude = 0,
                    LocationName = "My Location",
                    Longitude = 0
                },
                Title = "Cool party"
            });

            return result.Result;
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}