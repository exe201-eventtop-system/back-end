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
    public class CreateBlogCommand
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
    }

    public class CreateBlogResult: CreateBlogCommand
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
    }
    
    public class CreateBlogCommandHandler: ICommandHandler<CreateBlogCommand, Result<CreateBlogResult>>
    {
        private readonly IBlogRepository _repository;

        public CreateBlogCommandHandler(IBlogRepository repository) => _repository = repository;

        public async Task<Result<CreateBlogResult>> Handle(CreateBlogCommand command, CancellationToken cancellation)
        {
            Blog blog = new Blog
            {
                Title = command.Title,
                Description = command.Description,
                UserId = command.UserId,
            };

            blog = await _repository.CreateAsync(blog);

            return Result<CreateBlogResult>.Success(new CreateBlogResult
            {
                Id = blog.Id,
                Title = blog.Title,
                Description = command.Description,
                UserId = command.UserId,
            });
        }
    }
}
