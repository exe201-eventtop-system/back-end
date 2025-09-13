using Application.Commons.Models;
using Application.Commons.Queries;
using Domain.Repositories;
using SharedLibrary.DTOs.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Blogs.Queries
{
    public class GetBlogMinimalInfoSupQuery
    {
        public Guid IdSup { get; set; }
    }
    public class GetBlogMinimalInfoSupQueryHandler : IQueryHandler<GetBlogMinimalInfoSupQuery, Result<List<MinimalBlogInfo>>>
    {
        private readonly IBlogRepository blogRepository;

        public GetBlogMinimalInfoSupQueryHandler(IBlogRepository blogRepository)
        {
            this.blogRepository = blogRepository;
        }

        public async Task<Result<List<MinimalBlogInfo>>> Handle(GetBlogMinimalInfoSupQuery query, CancellationToken cancellation)
        {
            var result = await blogRepository.GetAllAsync();

            return Result<List<MinimalBlogInfo>>
                .Success(result.Where(x => x.UserId == query.IdSup).Select(x => new MinimalBlogInfo(x.Id, x.Title, x.UserId, x.CreatedAt, x.IsDeleted)).ToList(), "Success");
        }
    }
}
