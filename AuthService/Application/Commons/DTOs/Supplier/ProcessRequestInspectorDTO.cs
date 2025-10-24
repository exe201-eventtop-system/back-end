using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Commons.DTOs.Supplier
{
    public class ProcessRequestInspectorDTO 
    {
        public bool isAccept {  get; set; }
        public Guid Id  { get; set; }

        public IFormFile? Contract { get; set; } 

    }
}
