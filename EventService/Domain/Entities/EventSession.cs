using Domain.Constants.EventSessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class EventSession
    {
        public Guid Id { get; set; }

        public Guid EventId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Cost { get; set; }

        public DateTime StartTime { get; set; }

        public int Duration { get; set; }

        public DateTime EndTime => StartTime.AddMinutes(Duration);

        public EventSessionStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public virtual List<UsedServices> ServicesNavigation { get; set; }
    }
}
