using Application.Commons.DTOs.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.DTOs.User
{
    public class GetAllUserFillerDto : PaginationDto
    {
        public string Search { get; set; } = string.Empty;
    }
}
