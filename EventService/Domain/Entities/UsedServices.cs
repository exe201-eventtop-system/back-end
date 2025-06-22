using Domain.Constants;
using Domain.Constants.UsedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UsedServices
    {
        public Guid Id { get; set; }

        public Guid SessionId { get; set; }

        public Guid ServiceId { get; set; }

        public string ServiceName { get; set; }

        public string CustomerNote { get; set; }

        public decimal UnitPrice {  get; set; }

        public int Quantity { get; set; }

        public string InitialCondition { get; set; }

        public string ReturnedCondition { get; set; }

        public ServiceDamageType DamageType { get; set; }

        public UsedServiceStatus Status { get; set; }

        public DateTime? DeliveredTime { get; set; }

        public DateTime? ReturnTime { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }
    }
}
