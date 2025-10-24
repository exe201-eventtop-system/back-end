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
    public class UpdateBlogImageCommand
    {
        [JsonPropertyName("images")]
        public List<string> ImageUrls { get; set; }

        [JsonIgnore]
        public Guid BlogId;
    }

    public class UpdateBlogImageResult : UpdateBlogImageCommand
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("image_ids")]
        public List<Guid> ImageIds { get; set; }
    }

    public class UpdateBlogImageCommandHandler : ICommandHandler<UpdateBlogImageCommand, Result<UpdateBlogImageResult>>
    {
        private IBlogRepository _repository;

        public UpdateBlogImageCommandHandler(IBlogRepository repository) => _repository = repository;

        public async Task<Result<UpdateBlogImageResult>> Handle(UpdateBlogImageCommand command, CancellationToken cancellation)
        {
            var blog = await _repository.GetByIdAsync(command.BlogId);

            if (blog == null)
            {
                return Result<UpdateBlogImageResult>
                    .Failure(Error.NotFoundError($"can not find blog with id {command.BlogId}"));
            }

            blog.ImagesNavigation.Clear();

            foreach (string img in command.ImageUrls)
            {
                blog.ImagesNavigation.Add(new BlogImage
                {
                    BlogId = blog.Id,
                    ImageUrl = img,
                });
            }

            blog = await _repository.Update(blog);

            return Result<UpdateBlogImageResult>.Success(new UpdateBlogImageResult
            {
                Total = blog.ImagesNavigation.Count(),
                ImageUrls = command.ImageUrls,
                BlogId = blog.Id,
                ImageIds = blog.ImagesNavigation.Select(x => x.Id).ToList()
            });
        }
    }
}
