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
    public class UpdateBlogCommand
    {
        [JsonIgnore]
        public Guid BlogId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class UpdateBlogResult
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class UpdateBlogCommandHandler : ICommandHandler<UpdateBlogCommand, Result<UpdateBlogResult>>
    {
        private readonly IBlogRepository repository;

        public UpdateBlogCommandHandler(IBlogRepository repository) => this.repository = repository;

        public async Task<Result<UpdateBlogResult>> Handle(UpdateBlogCommand command, CancellationToken cancellation)
        {
            var blog = await repository.GetByIdAsync(command.BlogId);

            if (blog == null || blog.IsDeleted)
            {
                return Result<UpdateBlogResult>.Failure(Error.NotFoundError($"Can not find blog with id {command.BlogId}"));
            }

            blog.Title = command.Title;

            blog = await repository.Update(blog);

            return Result<UpdateBlogResult>.Success(new UpdateBlogResult
            {
                Title = blog.Title,
                Description = blog.Description
            });
        }
    }
}
