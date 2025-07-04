using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.DTOs.Supplier
{
    public class ProcessRequestInspectorDTO : ProcessRequestDTO
    {
        [Column("contract")]
        public IFormFile Contract { get; set; } 

    }
}
