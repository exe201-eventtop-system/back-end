using API.Extensions;
using Application.Commons.Dispatchers.Commands;
using Application.Commons.Dispatchers.Queries;
using Application.Commons.Results;
using Application.PackageStructures.Commands;
using Application.PackageStructures.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Jwt;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/package-structure")]
    [ApiController]
    public class PackageStructureController : ControllerBase
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly JwtService _jwtService;

        public PackageStructureController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
            _jwtService =  new JwtService(null, null);
        }

        [HttpGet]
        public async Task<IActionResult> GetStructureList(CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<GetStructureListCommand, Result<GetStructureListResult>>(new GetStructureListCommand(), cancellationToken);
            return result.MapToJsonResult();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateStructure([FromBody] CreatePackageStructureCommand command, CancellationToken cancellationToken)
        {
            command.AdminId = await _jwtService.ExtractUserIdFromToken(Request.Headers.Authorization[0].ToString());
            var result = await _commandDispatcher.Dispatch<CreatePackageStructureCommand, Result<CreatePackageStructureResult>>(command, cancellationToken);
            return result.MapToJsonResult();
        }

    }
}
