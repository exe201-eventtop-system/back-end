using Application.Commons.Commands;
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
    public class GetAllBlogQuery
    {
        [JsonPropertyName("page")]
        public int Page { get; set; } = 1;

        [JsonPropertyName("page_size")]
        public int PageSize { get; set; } = 10;

        [JsonPropertyName("search")]
        public string? TitleContain { get; set; } = null;
    }

    public class BlogQueryResult
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

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("update_at")]
        public DateTime LastModifiedAt { get; set; }
    }

    public class GetAllBlogQueryHandler : IQueryHandler<GetAllBlogQuery, Result<PaginatedList<BlogQueryResult>>>
    {
        private readonly IBlogRepository _repository;

        public GetAllBlogQueryHandler(IBlogRepository repository) => _repository = repository;

        public async Task<Result<PaginatedList<BlogQueryResult>>> Handle(GetAllBlogQuery query, CancellationToken cancellation)
        {
            var result = await _repository
                .GetAllAsync(filter: x => (string.IsNullOrEmpty(query.TitleContain) || x.Title.ToLower().Contains(query.TitleContain.ToLower())) && !x.IsDeleted, null);

            return Result<PaginatedList<BlogQueryResult>>.Success(new PaginatedList<BlogQueryResult>
            {
                CurrentPage = query.Page,
                PageSize = query.PageSize,
                PageCount = 0,
                TotalCount = result.Count,
                PageContent = result.Select(blog => new BlogQueryResult
                {
                    Id = blog.Id,
                    Title = blog.Title,
                    Description = blog.Description,
                    ThumbnailUrl = blog.ThumbnailUrl,
                    AuthorId = blog.UserId,
                    CreatedAt = blog.CreatedAt,
                    LastModifiedAt = blog.LastModifiedAt
                }).ToList()
            });
        }
    }
}
