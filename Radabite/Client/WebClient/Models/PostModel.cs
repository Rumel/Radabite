using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radabite.Client.WebClient.Models
{
    public class PostModel
    {
        public double providerId { get; set; }
        public string message { get; set; }
        public string fromName { get; set; }
        public double fromId { get; set; }

        //http://james.newtonking.com/json/help/index.html?topic=html/T_Newtonsoft_Json_Linq_JObject.htm to try to figure it out
        //public DateTimeOffset created_time { get; set; }
        public DateTimeOffset created_time { get; set; }
    }
}
