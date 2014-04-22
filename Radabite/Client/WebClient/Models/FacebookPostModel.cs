using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Radabite.Client.WebClient.Models
{
    public class FacebookPostModel : PostModel
    {

        public string photoUrl { get; set; }
        public byte[] photoBytes { get; set; }
       // public FacebookPostModel[] comments { get; set; }
    }

}