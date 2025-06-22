using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constants.EventSessions
{
    public enum EventSessionStatus
    {
        Scheduled,
        Started,
        Finished,
        Canceled
    }
}
