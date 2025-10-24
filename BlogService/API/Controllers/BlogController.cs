using Application.Blogs.Commands;
using Application.Blogs.Queries;
using Application.Commons.Commands;
using Application.Commons.Models;
using Application.Commons.Queries;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.DTOs.Blog;

namespace API.Controllers
{
    [Route("api/blogs")]
    [ApiController]
    public class BlogController: ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public BlogController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<Result<PaginatedList<BlogQueryResult>>> GetAllBlog([FromQuery] GetAllBlogQuery query, CancellationToken cancelationToken)
        {
            return await _queryDispatcher.Dispatch<GetAllBlogQuery, Result<PaginatedList<BlogQueryResult>>>(query, cancelationToken);
        }

        [HttpGet("minimal")]
        public async Task<Result<List<MinimalBlogInfo>>> GetCount()
        {
            var result = await _queryDispatcher.Dispatch<GetBlogMinimalInfoQuery,Result<List<MinimalBlogInfo>>>(new GetBlogMinimalInfoQuery(), CancellationToken.None);
            return result;
        }
        [HttpGet("minimal/supplier/{SupplierId}")]
        public async Task<Result<List<MinimalBlogInfo>>> GetCountSup(Guid SupplierId)
        {
            var result = await _queryDispatcher.Dispatch<GetBlogMinimalInfoSupQuery, Result<List<MinimalBlogInfo>>>(new GetBlogMinimalInfoSupQuery{ IdSup = SupplierId },CancellationToken.None);
            return result;
        }
        [HttpGet("{id}")]
        public async Task<Result<BlogDetailResult>> GetBlogDetail([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var query = new GetBlogDetailQuery { BlogId = id};

            return await _queryDispatcher.Dispatch<GetBlogDetailQuery, Result<BlogDetailResult>>(query, cancellationToken);
        }

        [HttpPost]
        public async Task<Result<CreateBlogResult>> CreateBlog([FromBody] CreateBlogCommand command, CancellationToken cancellationToken)
        {
            command.UserId = Guid.NewGuid(); // Fake User Id
            /// TODO: make this so that user id is taken from the user jwt token (or cookie).

            return await _commandDispatcher.Dispatch<CreateBlogCommand, Result<CreateBlogResult>>(command, cancellationToken);
        }

        [HttpPost("{id}/images")]
        public async Task<Result<UploadBlogImageResult>> UploadBlogImage([FromRoute] Guid id, [FromBody] UploadBlogImageCommand command, CancellationToken cancellationToken)
        {
            command.BlogId = id;
            return await _commandDispatcher.Dispatch<UploadBlogImageCommand, Result<UploadBlogImageResult>>(command, cancellationToken);
        }

        [HttpPut("{id}")]
        public async Task<Result<UpdateBlogResult>> UpdateBlog([FromRoute] Guid id, [FromBody]  UpdateBlogCommand command, CancellationToken cancellationToken)
        {
            command.BlogId = id;
            return await _commandDispatcher.Dispatch<UpdateBlogCommand, Result<UpdateBlogResult>>(command, cancellationToken);
        }

        [HttpPut("{id}/images")]
        public async Task<Result<UpdateBlogImageResult>> UpdateBlogImage([FromRoute] Guid id, [FromBody] UpdateBlogImageCommand command, CancellationToken cancellationToken)
        {
            command.BlogId = id;
            return await _commandDispatcher.Dispatch<UpdateBlogImageCommand, Result<UpdateBlogImageResult>>(command, cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task<Result<DeleteBlogResult>> DeleteBlog([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteBlogCommand { Id = id };
            return await _commandDispatcher.Dispatch<DeleteBlogCommand, Result<DeleteBlogResult>>(command, cancellationToken);
        }
    }
}
