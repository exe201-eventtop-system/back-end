using Application.Common;
using Application.Contracts;
using Application.Models.DTO.Service;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Commons;
using Infrastructure.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<ServiceResult<int>> AddServiceAsync(Service Service)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<int>> AddServiceImage(Guid service_id, List<FileStream> image_files)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<int>> DeleteServiceAsync(Guid Service_id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<PaginationResult<ServiceSummaryDTO>>> GetAllServiceAsync(int page, int page_size, string name_contain, string category)
        {
            Expression<Func<Service, bool>> expression = service 
                => service.Name.ToLower().Contains(name_contain.ToLower()) && service.CategoryNavigation.Name.ToLower().Contains(category.ToLower());

            PaginationResult<Service> result = await _unitOfWork.ServiceRepository.GetPaginatedAsync(page, page_size, expression, x=> x.OrderBy(x => x.Name));

            PaginationResult<ServiceSummaryDTO> mapped_result = new PaginationResult<ServiceSummaryDTO>
            {
                CurrentPage = result.CurrentPage,
                ItemCount = result.ItemCount,
                PageCount = result.PageCount,
                PageSize = result.PageSize,
                Items = _mapper.Map<List<ServiceSummaryDTO>>(result.Items),
            };

            return ServiceResult<PaginationResult<ServiceSummaryDTO>>.Success(mapped_result);
        }

        public async Task<ServiceResult<List<ServiceSummaryDTO>>> GetAllServiceAsync(List<Guid> service_ids)
        {
            List<Service> result = await _unitOfWork.ServiceRepository.GetAllAsync(x => service_ids.Contains(x.Id), x => x.OrderBy(x => x.Name));

            Console.WriteLine(result[0].CategoryNavigation.Name);

            List<ServiceSummaryDTO> mapped_result = _mapper.Map<List<ServiceSummaryDTO>>(result);

            return ServiceResult<List<ServiceSummaryDTO>>.Success(mapped_result);
        }

        public async Task<ServiceResult<ServiceDetailDTO>> GetServiceAsync(Guid service_id)
        {
            Service? result = await _unitOfWork.ServiceRepository.GetByIdAsync(service_id); 

            if (result == null)
            {
                return ServiceResult<ServiceDetailDTO>.Failure(ServiceError.NotFound($"Can not find record for service of id {service_id}."));
            }

            return ServiceResult<ServiceDetailDTO>.Success(_mapper.Map<ServiceDetailDTO>(result));
        }

        public Task<ServiceResult<int>> RemoveServiceImage(Guid image_id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<int>> UpdateServiceAsync(Service Service)
        {
            throw new NotImplementedException();
        }
    }
}
