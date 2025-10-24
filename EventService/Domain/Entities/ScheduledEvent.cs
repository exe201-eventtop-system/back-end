using Domain.Constants.Events;
using SharedLibrary.Enum;
using SharedLibrary.System.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ScheduledEvent : BaseEntities
    {
        [Column("customer_id")]
        public Guid Customer_Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("location")]
        public string Location { get; set; }

        [Column("start_time")]
        public DateTime StartTime { get; set; }

        [Column("end_time")]
        public DateTime EndTime { get; set; }

        [Column("number_of_people")]
        public int NumberOfPeople { get; set; }

        [Column("main_color_tag")]
        public string MainColorTag { get; set; }

        [Column("event_status")]
        public ScheduledEventStatus EventStatus { get; set; }

        [Column("event_type")]
        public EventType EventType { get; set; }

        public virtual List<UsedService>? ServicesNavigation { get; set; }
    }

}
