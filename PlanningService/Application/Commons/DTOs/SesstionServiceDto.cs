using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.DTOs
{
    public class SesstionServiceDto
    {
        public Guid Id { get; set; }
        public Guid PlanningId { get; set; }
        public Guid? ServiceId { get; set; }

    }

}
