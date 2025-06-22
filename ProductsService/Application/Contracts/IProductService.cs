using Application.Common;
using Application.Models.DTO.Service;
using Domain.Entities;
using Infrastructure.Commons;

namespace Application.Contracts
{
    public interface IProductService
    {
        /// <summary>
        ///     Get all service using pagination and service's name as search parameter and service category as filtering option.
        /// </summary>
        /// <param name="page">the page index</param>
        /// <param name="page_size">the page size</param>
        /// <param name="name_contain">the service name that contain the specific string</param>
        /// <param name="category">the service category must match the said category</param>
        /// <returns><see cref="PaginationResult{ServiceSummaryDTO}"/> represent the query result</returns>
        public Task<ServiceResult<PaginationResult<ServiceSummaryDTO>>> GetAllServiceAsync(int page, int page_size, string name_contain, string category);

        /// <summary>
        ///     Get all services using service id.
        /// </summary>
        /// <param name="service_ids">list of service ids</param>
        /// <returns>A <see cref="List{ServiceSummaryDTO}"/> containing found services.</returns>
        public Task<ServiceResult<List<ServiceSummaryDTO>>> GetAllServiceAsync(List<Guid> service_ids);

        /// <summary>
        ///     Get a specific record of service through it's id.
        /// </summary>
        /// <param name="Service_id">the service id of type <see cref="Guid"/></param>
        /// <returns>A <see cref="ServiceDetailDTO"/>record represent the detail of a service</returns>
        public Task<ServiceResult<ServiceDetailDTO>> GetServiceAsync(Guid Service_id);

        public Task<ServiceResult<int>> AddServiceAsync(Service Service);

        public Task<ServiceResult<int>> UpdateServiceAsync(Service Service);

        public Task<ServiceResult<int>> DeleteServiceAsync(Guid Service_id);

        public Task<ServiceResult<int>> AddServiceImage(Guid service_id, List<FileStream> image_files);

        public Task<ServiceResult<int>> RemoveServiceImage(Guid image_id);
    }
}
