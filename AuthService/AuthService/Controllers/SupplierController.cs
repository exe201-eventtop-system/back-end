using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/suppliers")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly IUserUseCase _useCase;
        public SupplierController(IUserUseCase useCase)
        {
            _useCase = useCase;
        }
        [HttpPost("batch")]
        public async Task<IActionResult> GetSuppliersBatch([FromBody] List<Guid> supplierIds)
        {
            var suppliers = await _useCase.GetListSupllier(supplierIds);
            return Ok(suppliers);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplier(Guid id)
        {
            var supplier = await _useCase.GetSupllier(id);
            return Ok(supplier);
        }
    }
}
