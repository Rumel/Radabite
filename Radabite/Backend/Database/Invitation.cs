﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Radabite.Backend.Database
{
    public class Invitation : DataObject
    {
        public virtual User Guest { get; set; }

        public long GuestId { get; set; }

        public ResponseType Response { get; set; }

        public InvitationJson ToJson() 
        {
            return new InvitationJson
            {
                GuestId = Guest.Id,
                Response = Response.ToString()
            };
        }
    }

    public enum ResponseType
    {
        Accepted,
        Rejected,
        WaitingReply
    }

    public class InvitationJson
    {
        public long GuestId { get; set; }

        public String Response { get; set; }
    }
}