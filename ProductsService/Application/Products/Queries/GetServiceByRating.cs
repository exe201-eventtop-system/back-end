using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.Queries
{
    public class GetServiceByRatingQuery
    {
    }
    public class GetServiceByRatingQueryHandler : IQueryHandler<GetServiceByRatingQuery, Result<List<ProductDetail>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpClientFactory httpClientFactory;

        public GetServiceByRatingQueryHandler(IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<List<ProductDetail>>> Handle(GetServiceByRatingQuery query, CancellationToken cancellationToken)
        {
            var services = await unitOfWork.ProductRepository.GetAllAsync();
            var top10Service= services
               .OrderByDescending(s => s.CreatedAt)
               .Take(10)
               .Select(s => new ProductDetail
               {
                   Id = s.Id,
                   Name = s.Name,
                   Description = s.Description,
                   Category = s.CategoryNavigation?.Name,
                   ThumbnailUrl = s.ThumbnailUrl,

               })
               .ToList();

            return Result<List<ProductDetail>>.Success(top10Service);
        }
    }
}
