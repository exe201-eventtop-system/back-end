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
    public class GetBlogMinimalInfoQuery
    {
    }

    public class GetBlogMinimalInfoQueryHandler : IQueryHandler<GetBlogMinimalInfoQuery, Result<List<MinimalBlogInfo>>>
    {
        private readonly IBlogRepository blogRepository;

        public GetBlogMinimalInfoQueryHandler(IBlogRepository blogRepository)
        {
            this.blogRepository = blogRepository;
        }

        public async Task<Result<List<MinimalBlogInfo>>> Handle(GetBlogMinimalInfoQuery query, CancellationToken cancellation)
        {
            var result = await blogRepository.GetAllAsync();

            return Result<List<MinimalBlogInfo>>
                .Success(result.Select(x => new MinimalBlogInfo(x.Id, x.Title, x.UserId, x.CreatedAt, x.IsDeleted)).ToList(), "Success");
        }
    }
}
