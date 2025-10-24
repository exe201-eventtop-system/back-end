using Application.Commons.Handlers;
using Application.Commons.UoW;
using Domain.Entities;
using SharedLibrary.DTOs.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Feedbacks.Queries
{
    public class GetServiceRating : List<ServiceRatingDto> { }

    public class GetServiceRatingHandler : IQueryHandler<GetServiceRating, List<ServiceRatingDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetServiceRatingHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ServiceRatingDto>> Handle(GetServiceRating query, CancellationToken cancellationToken)
        {
            var feedbacks = await _unitOfWork.UsedServiceRepository.GetUsedServicesWithRating();

            if (feedbacks == null || !feedbacks.Any())
                return [];

            var serviceRatings = feedbacks
                .Where(f => f.ServiceId != Guid.Empty)
                .GroupBy(f => f.ServiceId)
                .Select(g => new ServiceRatingDto
                {
                    ServiceId = g.Key,
                    AverageRating = Math.Round(g.Average(f => f.Feedback.RatingService), 2)
                })
                .OrderByDescending(x => x.AverageRating)
                .Take(10)
                .ToList();

            return serviceRatings;
        }

    }
}
