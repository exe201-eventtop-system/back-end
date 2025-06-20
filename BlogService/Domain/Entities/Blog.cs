using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Blog
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string ThumbnailUrl { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public List<BlogImage> ImagesNavigation { get; set; }
    }
}
