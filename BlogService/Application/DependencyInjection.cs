using Application.Blogs.Commands;
using Application.Blogs.Queries;
using Application.Commons.Commands;
using Application.Commons.Dispatcher;
using Application.Commons.Models;
using Application.Commons.Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.DTOs.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add helper services.

            // Add dispatchers
            services.AddScoped<IQueryDispatcher, QueryDispatcher>();
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();

            // commands
            services.AddScoped<ICommandHandler<CreateBlogCommand, Result<CreateBlogResult>>, CreateBlogCommandHandler>();
            services.AddScoped<ICommandHandler<UploadBlogImageCommand, Result<UploadBlogImageResult>>, UploadBlogImageCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateBlogCommand, Result<UpdateBlogResult>>, UpdateBlogCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateBlogImageCommand, Result<UpdateBlogImageResult>>, UpdateBlogImageCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteBlogCommand, Result<DeleteBlogResult>>, DeleteBlogCommandHandler>();

            // queries
            services.AddScoped<IQueryHandler<GetAllBlogQuery, Result<PaginatedList<BlogQueryResult>>>, GetAllBlogQueryHandler>();
            services.AddScoped<IQueryHandler<GetBlogDetailQuery, Result<BlogDetailResult>>, GetBlogDetailQueryHandler>();
            services.AddScoped<IQueryHandler<GetBlogMinimalInfoQuery, Result<List<MinimalBlogInfo>>>,GetBlogMinimalInfoQueryHandler>();
            return services;
        }
    }
}
