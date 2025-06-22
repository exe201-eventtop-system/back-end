using Application.Common;
using Application.Contracts;
using Application.Models.DTO.Service;
using AutoMapper;
using Contacts.Supplier;
using Domain.Entities;
using Infrastructure.Commons;
using Infrastructure.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper,IHttpClientFactory httpClientFactory)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
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
                => service.Name.ToLower().Contains(name_contain.ToLower())
                && service.CategoryNavigation.Name.ToLower().Contains(category.ToLower());

            PaginationResult<Service> result = await _unitOfWork.ServiceRepository.GetPaginatedAsync(
                page, page_size, expression, x => x.OrderBy(x => x.Name));

            List<ServiceSummaryDTO> serviceDtos = _mapper.Map<List<ServiceSummaryDTO>>(result.Items);

            List<Guid> supplierIds = result.Items
                .Where(s => s.SupplierId != Guid.Empty)
                .Select(s => s.SupplierId)
                .Distinct()
                .ToList();

            List<SupplierResponseDTO> suppliers = new List<SupplierResponseDTO>();
            if (supplierIds.Any())
            {
                try
                {
                    HttpClient httpClient = _httpClientFactory.CreateClient("AuthService");
                    HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/suppliers/batch", supplierIds);
                    if (response.IsSuccessStatusCode)
                    {
                        suppliers = await response.Content.ReadFromJsonAsync<List<SupplierResponseDTO>>();
                    }
                }
                catch (Exception)
                {
                }
            }

            Dictionary<Guid, SupplierResponseDTO> supplierDict = suppliers.ToDictionary(s => s.Id, s => s);

            foreach (ServiceSummaryDTO dto in serviceDtos)
            {
                Service service = result.Items.FirstOrDefault(s => s.Id == dto.Id);
                if (service != null && service.SupplierId != Guid.Empty && supplierDict.ContainsKey(service.SupplierId))
                {
                    SupplierResponseDTO supplier = supplierDict[service.SupplierId];
                    dto.SupplierName = supplier.Name ?? "Unknown";
                    dto.IsActive = supplier.IsActive;
                }
                else
                {
                    dto.SupplierName = "Unknown";
                    dto.IsActive = false; 
                }
            }

            PaginationResult<ServiceSummaryDTO> mapped_result = new PaginationResult<ServiceSummaryDTO>
            {
                CurrentPage = result.CurrentPage,
                ItemCount = result.ItemCount,
                PageCount = result.PageCount,
                PageSize = result.PageSize,
                Items = serviceDtos
            };

            return ServiceResult<PaginationResult<ServiceSummaryDTO>>.Success(mapped_result);
        }
        public async Task<ServiceResult<ServiceDetailDTO>> GetServiceAsync(Guid service_id)
        {
            //Service? result = await _unitOfWork.ServiceRepository.GetByIdAsync(service_id); 

            //if (result == null)
            //{
            //    return ServiceResult<ServiceDetailDTO>.Failure(ServiceError.NotFound($"Can not find record for service of id {service_id}."));
            //}

            //return ServiceResult<ServiceDetailDTO>.Success(_mapper.Map<ServiceDetailDTO>(result));
            Service? result = await _unitOfWork.ServiceRepository.GetByIdAsync(service_id);

            SupplierResponseDTO? supplierInfo = null;

            if (result.SupplierId != Guid.Empty)
            {
                try
                {
                    var httpClient = _httpClientFactory.CreateClient("AuthService");
                    var response = await httpClient.GetAsync($"api/suppliers/{result.SupplierId}");

                    if (response.IsSuccessStatusCode)
                    {
                        supplierInfo = await response.Content.ReadFromJsonAsync<SupplierResponseDTO>();
                    }
                }
                catch (Exception ex)
                {
                    // Log nếu cần
                }
            }

            var dto = _mapper.Map<ServiceDetailDTO>(result);

            // Đảm bảo dto.Supplier đã được khởi tạo
            dto.Supplier ??= new Supplier();
            dto.SupplierName = supplierInfo?.Name ?? "Unknown";
            dto.Supplier.Name = supplierInfo?.Name ?? "Unknown";
            dto.Supplier.Avatar = supplierInfo?.Avatar ?? string.Empty;
            dto.Supplier.Location = supplierInfo?.Location ?? "Unknown";
            dto.Supplier.IsActive = supplierInfo?.IsActive ?? false;
            dto.SupplierId = result.SupplierId ;

            return ServiceResult<ServiceDetailDTO>.Success(dto);

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
