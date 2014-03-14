using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Radabite.Client.WebClient.Models
{
    public class FacebookPostModel
    {
        public string message { get; set; }
        public string from { get; set; }
        //   public DateTimeOffset updated_time { get; set; }  http://james.newtonking.com/json/help/index.html?topic=html/T_Newtonsoft_Json_Linq_JObject.htm to try to figure it out
        // public IList<FacebookPostModel> comments { get; set; }
    }
}