using Application.Commons.DTOs.Supplier;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Jwt;
using Application.Commons;
using SharedLibrary.DTOs.Supplier;

namespace API.Controllers
{
    [Route("api/suppliers")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly IUserUseCase _useCase;
        private readonly JwtService _jwtService;
        public SupplierController(IUserUseCase useCase, JwtService jwtService)
        {
            _useCase = useCase;
            _jwtService = jwtService;
        }
        [HttpPost("batch")]
        public async Task<IActionResult> GetSuppliersBatch([FromBody] List<Guid> supplierIds)
        {
            var suppliers = await _useCase.GetListSupllier(supplierIds);
            return Ok(suppliers);
        }

        [HttpGet("by-rating")]
        public async Task<IActionResult> GetSuppliersByRating()
        {
            var supplier = await _useCase.GetSuppliersByRating();
            return Ok(supplier);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplier(Guid id)
        {
            var supplier = await _useCase.GetSupllier(id);
            return Ok(supplier);
        }
        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetSupplierDetail(Guid id)
        {
            var supplier = await _useCase.GetSupplierDetail(id);
            return Ok(supplier);
        }
        //[HttpPost("sign-up")]
        //public async Task<IActionResult> SignUpSupplier(SignUpSupplierDTO signUpSupplierDTO)
        //{
        //    var supplier = await _useCase.SignUpSupplier(signUpSupplierDTO);
        //    return Ok(supplier);
        //}
        [HttpGet]
        public async Task<IActionResult> GetSuppliers([FromQuery] SupplierFilterDto filter) => (await _useCase.GetSuppliers(filter)).ToActionResult();


        //[HttpGet("inspector")]
        //public async Task<IActionResult> GetSuppliersInspect([FromQuery] SupplierFilterPagingDTO filter)
        //{
        //    var token = HttpContext.Request.Headers["Authorization"].ToString();

        //    Guid userId = await _jwtService.ExtractUserIdFromToken(token);
        //    var supplier = await _useCase.GetSuppliersInspect(userId);
        //    return Ok(supplier);
        //}
        [HttpPost("process-request/admin")]
        public async Task<IActionResult> ProcessRequestAdmin(ProcessRequestDTO processRequestDTO)
        {
            var supplier = await _useCase.ProcessRequestAsync(processRequestDTO);
            return Ok(supplier);
        }

        [HttpPost("process-request/inspector")]
        public async Task<IActionResult> ProcessRequestInspector(ProcessRequestInspectorDTO processRequestDTO)
        {
            var supplier = await _useCase.ProcessRequestInspectorAsync(processRequestDTO);
            return Ok(supplier);
        }

        [HttpPost("{id}/balances")]
        public async Task<IActionResult> ProccessBalanceUpdate([FromRoute] Guid id , SupplierBalanceUpdateDto command)
        {
            var result = await _useCase.UpdateSupplierBalance(id, command.Amount);
            return result.ToActionResult();
        }

    }
}
