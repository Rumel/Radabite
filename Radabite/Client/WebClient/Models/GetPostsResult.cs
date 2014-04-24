using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Radabite.Client.WebClient.Models
{
    public class GetPostsResult
    {
        public bool hasErrors { get; set; }
        public string errorMessage { get; set; }
        public IEnumerable<PostModel> posts { get; set; }
    }
}