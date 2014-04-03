using Radabite.Backend.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Radabite.Backend.Database
{
    public class Event : DataObject
    {
        public string Title { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsPrivate { get; set; }

        public string Description { get; set; }

        public Location Location { get; set; }

        public bool FinishedGettingPosts { get; set; }

        public bool IsActive { get; set; }

        public virtual User Owner { get; set; }

        public virtual ICollection<Invitation> Guests { get; set; }

        public EventJson ToJson()
        {
            var ej = new EventJson
                        {
                            Id = Id,
                            Title = Title,
                            StartTime = StartTime,
                            EndTime = EndTime,
                            IsPrivate = IsPrivate,
                            Description = Description,
                            Location = Location,
                            FinishedGettingPosts = FinishedGettingPosts,
                            IsActive = IsActive,
                            OwnerId = Owner.Id,
                        };

            if(Guests == null) {
                ej.Guests = new InvitationJson[0];
            }
            else
            {
                ej.Guests = Guests.Select(x => x.ToJson()).ToArray();
            }

            return ej;
        }
    }

    public class EventJson
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsPrivate { get; set; }

        public string Description { get; set; }

        public Location Location { get; set; }

        public bool FinishedGettingPosts { get; set; }

        public bool IsActive { get; set; }

        public long OwnerId { get; set; }

        public InvitationJson[] Guests { get; set; }
    }
}