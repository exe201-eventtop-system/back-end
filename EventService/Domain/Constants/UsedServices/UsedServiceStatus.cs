using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constants.UsedServices
{
    public enum UsedServiceStatus
    {
        Registered,
        Delivered,
        Recieved,
        Returned,
        ReturnedAccepted,
        Canceled
    }
}
