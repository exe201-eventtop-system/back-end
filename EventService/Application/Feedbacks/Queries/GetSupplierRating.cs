using Application.Commons.Handlers;
using Application.Commons.UoW;
using Domain.Entities;
using SharedLibrary.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Feedbacks.Queries
{
    public class GetSupplierRating : List<SupplierRatingDto> { }

    public class GetSupplierRatingHandler : IQueryHandler<GetSupplierRating, List<SupplierRatingDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSupplierRatingHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SupplierRatingDto>> Handle(GetSupplierRating query, CancellationToken cancellationToken)
        {
            var usedServices = await _unitOfWork.UsedServiceRepository.GetUsedServicesWithRating();

            if (usedServices == null || !usedServices.Any())
            {
                return [];
            }

            var supplierRatings = usedServices
                .Where(us => us.SupplierId != Guid.Empty && us.Feedback != null)
                .GroupBy(us => us.SupplierId)
                .Select(g => new SupplierRatingDto
                {
                    SupplierId = g.Key,
                    AverageRating = Math.Round(g.Average(us => us.Feedback.RatingSupplier), 2)
                })
                .OrderByDescending(x => x.AverageRating)
                .Take(10)
                .ToList();

            return supplierRatings;
        }


    }
}
