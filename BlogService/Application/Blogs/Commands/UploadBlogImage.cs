using Application.Commons.Commands;
using Application.Commons.Models;
using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Blogs.Commands
{
    public class UploadBlogImageCommand
    {
        [JsonPropertyName("images")]
        public List<string> ImageUrls { get; set; }

        [JsonIgnore]
        public Guid BlogId;
    }

    public class UploadBlogImageResult: UploadBlogImageCommand
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("image_ids")]
        public List<Guid> ImageIds { get; set; }
    }

    public class UploadBlogImageCommandHandler: ICommandHandler<UploadBlogImageCommand, Result<UploadBlogImageResult>>
    {
        private IBlogRepository _repository;

        public UploadBlogImageCommandHandler(IBlogRepository repository) => _repository = repository;

        public async Task<Result<UploadBlogImageResult>> Handle(UploadBlogImageCommand command, CancellationToken cancellation)
        {
            var blog = await _repository.GetByIdAsync(command.BlogId);

            if (blog == null)
            {
                return Result<UploadBlogImageResult>
                    .Failure(Error.NotFoundError($"can not find blog with id {command.BlogId}"));
            }

            foreach (string img in command.ImageUrls)
            {
                blog.ImagesNavigation.Add(new BlogImage
                {
                    BlogId = blog.Id,
                    ImageUrl = img,
                });
            }
            
            blog.LastModifiedAt = DateTime.UtcNow;
            blog = await _repository.Update(blog);

            return Result<UploadBlogImageResult>.Success(new UploadBlogImageResult
            {
                Total = blog.ImagesNavigation.Count(),
                ImageUrls = command.ImageUrls,
                BlogId = blog.Id,
                ImageIds = blog.ImagesNavigation.Select(x => x.Id).ToList()
            });
        }
    }
}
