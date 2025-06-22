using Domain.Constants.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ScheduledEvent
    {
        public Guid Id { get; set; }

        public Guid CreatorId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public DateOnly StartDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public DateOnly EndDate { get; set; }

        public TimeOnly EndTime { get; set; }

        public int NumberOfPeople { get; set; }

        public string MainColorTag { get; set; }

        public string SecondaryColorTag { get; set; }

        public int? EventTypeId { get; set; }

        public ScheduledEventStatus EventStatus { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public virtual EventType EventTypeNavigation { get; set; }

        public virtual List<EventSession> SessionsNavigation { get; set; }
    }
}
