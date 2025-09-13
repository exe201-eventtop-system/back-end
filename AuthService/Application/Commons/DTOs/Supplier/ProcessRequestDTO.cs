using SharedLibrary.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Commons.DTOs.Supplier
{
    public class ProcessRequestDTO
    {
        public Guid Id { get; set; }
        [Column("email")]
        public string? Email { get; set; }

        [Column("phone_number")]
        public string? PhoneNumber { get; set; }
        [Column("location_orginazation")]
        public string Location { get; set; } = string.Empty;
        [Column("name_organization")]
        public string NameOrginazation { get; set; } = string.Empty;
        [Column("description")]
        public string? Description { get; set; }
        [Column("about")]
        public string? About { get; set; }
        [Column("business_license")]
        public string? BusinessLicense { get; set; }
        [Column("tax_code")]
        public string TaxCode { get; set; } = string.Empty;
        [Column("thumnnail")]
        public string Thumnnail { get; set; } = string.Empty;
        public virtual ICollection<string> OrginazationImages { get; set; } = new List<string>();
    }
}