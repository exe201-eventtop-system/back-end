namespace Domain.Entities
{
    public class EventType
    {
        public int Id { get; set; }

        public string ThumbnailUrl { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public virtual List<ScheduledEvent> ScheduledEventsNavigation { get; set; }
    }
}
