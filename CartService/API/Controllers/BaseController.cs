
using Microsoft.AspNetCore.Mvc;
using Services.Commons;
using SharedLibrary.TokenUtilities;
using ShareLibary.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

[ApiController]
public class BaseController : ControllerBase
{
    private const string ERORR_MESSAGE = "An error has occurred:";
    protected readonly ITokenUtilities _tokenUtils;

    public BaseController() { }

    public BaseController( ITokenUtilities tokenUtilities)
    {
        _tokenUtils = tokenUtilities;
    }

    private ActionResult? HandleError(ServiceResult result)
    {
        if (result.IsFailed)
        {
            ApiResponse errorResponse = new ApiResponse(false, result.Error.Description!, result.Error.Information, result.Error.Code);

            switch (result.Error.Code)
            {
                case ServiceError._ValidationFailed:
                    return StatusCode(400, errorResponse);
                case ServiceError._NotExisted:
                    return StatusCode(400, errorResponse);
                case ServiceError._Unauthorized:
                    return StatusCode(401, errorResponse);
                case ServiceError._NotFound:
                    return StatusCode(404, errorResponse);
                case ServiceError._AlreadyExisted:
                    return StatusCode(409, errorResponse);
                default:
                    return StatusCode(500, errorResponse);
            }
        }
        return null;
    }

    protected string GetUserIdFromJwt()
    {
        string? jwt = Request.Headers.Authorization.First();

        // Throw an error when this method is used with an endpoint does not requires authentication.
        if (jwt == null)
        {
            throw new InvalidOperationException("Can not use this method with endpoint allows anonymous access.");
        }

        // Split the bearer token ("Bearer adfbnenofcsa...") into two parts and take the jwt part to decode and get the user id.
        return _tokenUtils.GetDataDictionaryFromJwt(jwt.Split()[1])["id"];
    }
    protected string GetUserRoleFromJwt()
    {
        string? jwt = Request.Headers.Authorization.First();
        if (jwt == null)
        {
            throw new InvalidOperationException("Can not use this method with endpoint allows anonymous access.");
        }

        return _tokenUtils.GetDataDictionaryFromJwt(jwt.Split()[1])["role"];
    }
    protected async Task<IActionResult> HandleApiCallAsync<T>(Func<Task<T>> func)
    {
        try
        {
            var result = await func();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ERORR_MESSAGE, error = ex.Message });
        }
    }

    protected ActionResult HandleServiceCall(ServiceResult result, string success_message = "Success")
    {
        var ErrorResponse = HandleError(result);

        if (ErrorResponse != null)
        {
            return ErrorResponse;
        }

        return Ok(new ApiResponse(true, success_message, result.Data));
    }

    protected async Task<ActionResult> HandleServiceCall(Func<Task<ServiceResult>> func)
    {
        var result = await func();

        var error = HandleError((ServiceResult)result);

        if (error != null)
        {
            return error;
        }

        return Ok(new ApiResponse(true, "Success", result.Data));
    }

    protected async Task<ActionResult> HandleServiceCall<T>(Func<Task<ServiceResult>> func)
    {
        var result = await func();

        var error = HandleError((ServiceResult)result);

        if (error != null)
        {
            return error;
        }

        return Ok(new ApiResponse(true, "Success", result.Data));
    }

}
