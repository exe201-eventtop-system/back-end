using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.DTOs.AI
{
    public class PlanningAIResponseDTO
    {
        public string EventName { get; set; }
        public string EventDate { get; set; }
        public string Location { get; set; }
        public string ExpectedParticipants { get; set; }
        public List<string> ThemeColor { get; set; }
        public string Budget { get; set; }
        public string Description { get; set; }
        public string EventType { get; set; }
    }
}
