using Application.Commons.Models;
using Application.Commons.Queries;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Blogs.Queries
{
    public class GetBlogDetailQuery
    {
        public Guid BlogId { get; set; }
    }

    public class BlogDetailResult
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("thumbnail")]
        public string ThumbnailUrl { get; set; }

        [JsonPropertyName("supplier_id")]
        public Guid AuthorId { get; set; }

        [JsonPropertyName("images")]
        public List<string> ImageUrls { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("update_at")]
        public DateTime LastModifiedAt { get; set; }

    }

    public class GetBlogDetailQueryHandler : IQueryHandler<GetBlogDetailQuery, Result<BlogDetailResult>>
    {
        private IBlogRepository _repository;

        public GetBlogDetailQueryHandler(IBlogRepository repository) => _repository = repository;

        public async Task<Result<BlogDetailResult>> Handle(GetBlogDetailQuery query, CancellationToken cancellation)
        {
            var blog = await _repository.GetByIdAsync(query.BlogId);

            if (blog == null || blog.IsDeleted)
            {
                return Result<BlogDetailResult>.Failure(Error.NotFoundError($"Can not find blog with id {query.BlogId}."));
            }

            return Result<BlogDetailResult>.Success(new BlogDetailResult
            {
                Id = query.BlogId,
                Title = blog.Title,
                Description = blog.Description,
                AuthorId = blog.UserId,
                ThumbnailUrl = blog.ThumbnailUrl,
                CreatedAt = blog.CreatedAt,
                LastModifiedAt = blog.LastModifiedAt,
                ImageUrls = blog.ImagesNavigation.Select(x => x.ImageUrl).ToList()
            });
        }
    }
}
