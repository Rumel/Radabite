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
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
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