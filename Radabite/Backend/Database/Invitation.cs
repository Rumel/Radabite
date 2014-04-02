using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Radabite.Backend.Database
{
    public class Invitation : DataObject
    {
        public User Guest { get; set; }

        public ResponseType Response { get; set; }
    }

    public enum ResponseType
    {
        Accepted,
        Rejeced,
        WaitingReply
    }
}