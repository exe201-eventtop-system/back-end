using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.DTOs.Supplier
{
    public class SignUpSupplierDTO
    {
        public string Email { get; set; } = string.Empty;
        [Column("phone_number")]
        public string PhoneNumber { get; set; } = string.Empty;
        [Column("location_orginazation")]
        public string Location { get; set; } = string.Empty;
        [Column("name_organization")]
        public string NameOrginazation { get; set; } = string.Empty;
        [Column("tax_code")]
        public string TaxCode { get; set; } = string.Empty;
        [Column("description")]
        public string Description { get; set; }
        [Column("about")]
        public string About { get; set; }
        [Column("business_license")]
        public string BusinessLicense { get; set; }
        [Column("thumnnail")]
        public string Thumnnail { get; set; } = string.Empty;
        public ICollection<IFormFile> formFiles { get; set; } = new List<IFormFile>();
    }
}
