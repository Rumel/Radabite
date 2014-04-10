using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Radabite.Client.WebClient.Models
{
    public class FacebookGetPostsResult
    {
        public bool hasErrors { get; set; }
        public string errorMessage { get; set; }
        public IList<FacebookPostModel> posts { get; set; }
    }
}