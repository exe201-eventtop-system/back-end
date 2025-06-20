using Application.Commons.Commands;
using Application.Commons.Models;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Blogs.Commands
{
    public class DeleteBlogCommand
    {
        public Guid Id { get; set; }
    }

    public class DeleteBlogResult
    {
    }

    public class DeleteBlogCommandHandler : ICommandHandler<DeleteBlogCommand, Result<DeleteBlogResult>>
    {
        private readonly IBlogRepository repository;

        public DeleteBlogCommandHandler(IBlogRepository repository) => this.repository = repository;


        public async Task<Result<DeleteBlogResult>> Handle(DeleteBlogCommand command, CancellationToken cancellation)
        {
            var blog = await repository.GetByIdAsync(command.Id);

            if (blog == null || blog.IsDeleted)
            {
                return Result<DeleteBlogResult>.Failure(Error.NotFoundError($"Can not find blog for id {command.Id}"));
            }

            blog.IsDeleted = true;

            blog = await repository.Update(blog);

            return Result<DeleteBlogResult>.Success();
        }
    }
}
