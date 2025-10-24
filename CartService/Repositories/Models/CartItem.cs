using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    public partial class CartItem : BaseModel
    {
        [Column("cart_id")]
        [ForeignKey(nameof(Cart))]
        public Guid CartId { get; set; }

        [Column("service_id")]
        public Guid ServiceId { get; set; }

        public virtual Cart? Carts { get; set; }
    }
}
