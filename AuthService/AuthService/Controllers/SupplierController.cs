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
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUpSupplier(SignUpSupplierDTO signUpSupplierDTO)
        {
            var supplier = await _useCase.SignUpSupplier(signUpSupplierDTO);
            return Ok(supplier);
        }
        [HttpPost("sign-up/update-license")]
        public async Task<IActionResult> UpdateLicense([FromForm] SignUpLicenseSupplierDTO signUpSupplierDTO)
        {
            var supplier = await _useCase.UpdateLicense(signUpSupplierDTO);
            return Ok(supplier);
        }
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
        [HttpGet("process-request")]
        public async Task<IActionResult> ProcessRequest()
        {
            var supplier = await _useCase.ProcessRequestAsync();
            return Ok(supplier);
        }

        [HttpPost("process-request")]
        public async Task<IActionResult> ProcessRequestInspector([FromForm]ProcessRequestInspectorDTO processRequestDTO)
        {
            var supplier = await _useCase.ProcessRequestInspectorAsync(processRequestDTO);
            return Ok(supplier);
        }
        [HttpGet("{id}/balances/{amount}")]
        public async Task<IActionResult> GetBalanceUpdate([FromRoute] Guid id, decimal amount)
        {
            var result = await _useCase.UpdateSupplierBalance(id, amount);
            return Ok(result);
        }


    }
}
