
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.DTOs.Blog
{
    public record MinimalBlogInfo(Guid Id, string Title, DateTime CreatedAt, bool IsDeleted);
}
