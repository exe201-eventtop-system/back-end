using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.DTOs.Service
{
    public record MinimalServiceInfo(Guid Id, Guid SupplierId, string Name, DateTime createdDate, bool IsDeleted);
}
