using Radabite.Backend.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Radabite.Models
{
    public class InvitationModel
    {
        public User User { get; set; }

        public long EventId { get; set; }
    }
}