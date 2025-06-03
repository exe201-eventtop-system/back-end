using API.Common;
using Application.Common;
using Application.Contracts;
using Application.Models.DTO.Category;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service) => _service = service;

        [HttpGet]
        [ProducesDefaultResponseType(typeof(ApiResponse<List<CategoryDTO>>))]
        [ProducesResponseType(400,Type = typeof(ApiResponse<object>))]
        public async Task<IActionResult> GetAll()
        {
            ServiceResult<List<CategoryDTO>> result = await _service.GetAllCategoryAsync();

            if (result.IsFailure)
            {
                return Ok(new ApiResponse<object>
                {
                    IsSuccess = false,
                    ErrorCode = result.Error.ErrorCode,
                    Message = result.Error.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponse<List<CategoryDTO>>
            {
                IsSuccess = true,
                ErrorCode = null,
                Message = "Success",
                Data = result.Value
            });
        }
    }
}
