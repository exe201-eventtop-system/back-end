using API.Common;
using Application.Common;
using Application.Contracts;
using Application.Models.DTO.Service;
using Infrastructure.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/services")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IProductService _service;

        public ServicesController(IProductService service) => _service = service;

        [HttpGet]
        [ProducesResponseType(typeof(PaginationResult<ServiceSummaryDTO>), 200)]
        public async Task<IActionResult> GetAllAsync([FromQuery] int page = 1, [FromQuery] int page_size = 5, string search = "", string category = "")
        {
            ServiceResult<PaginationResult<ServiceSummaryDTO>> result = await _service.GetAllServiceAsync(page, page_size, search, category);

            return Ok(new ApiResponse<PaginationResult<ServiceSummaryDTO>>
            {
                IsSuccess = true,
                ErrorCode = null,
                Message = "Success",
                Data = result.Value
            });
        }

        [HttpPost("ids")]
        [ProducesResponseType(typeof(List<ServiceSummaryDTO>), 200)]
        public async Task<IActionResult> GetAllWithIdListAsync([FromBody] List<Guid> product_ids)
        {
            ServiceResult<List<ServiceSummaryDTO>> result = await _service.GetAllServiceAsync(product_ids);

            return Ok(new ApiResponse<List<ServiceSummaryDTO>>
            {
                IsSuccess = true,
                ErrorCode = null,
                Message = "Success",
                Data = result.Value
            });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PaginationResult<ServiceDetailDTO>), 200)]
        [ProducesResponseType(typeof(PaginationResult<object>), 404)]
        public async Task<IActionResult> GetDetailAsync(Guid id)
        {
            ServiceResult<ServiceDetailDTO> result = await _service.GetServiceAsync(id);

            if (result.IsFailure)
            {
                if (result.Error.ErrorCode == ServiceError.NotFoundErrorCode)
                {
                    return StatusCode(404, new ApiResponse<object>
                    {
                        IsSuccess = false,
                        ErrorCode = result.Error.ErrorCode,
                        Message = result.Error.Message,
                        Data = null
                    });
                }
            }

            return Ok(new ApiResponse<ServiceDetailDTO>
            {
                IsSuccess = true,
                ErrorCode = null,
                Message = "Success",
                Data = result.Value
            });
        }

    }
}
