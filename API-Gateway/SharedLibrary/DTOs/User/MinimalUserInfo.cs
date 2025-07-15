using SharedLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.DTOs.User
{
    public record MinimalUserInfo(Guid Id, string Username, UserRole Role, string? SupplierName, DateTime CreatedDate, bool IsDeleted);

}
