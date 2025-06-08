using Microsoft.AspNetCore.Mvc;

namespace Application.Commons;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(new ApiResponse
            {
                Success = true,
                Message = "Success"
            });
        }

        return new BadRequestObjectResult(new ApiResponse
        {
            Success = false,
            Message = result.Error?.Description ?? "An error occurred",
            ErrorCode = result.Error?.Code
        });
    }

    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(new ApiResponse<T>
            {
                Success = true,
                Message = "Success",
                Data = result.Value
            });
        }

        return new BadRequestObjectResult(new ApiResponse<T>
        {
            Success = false,
            Message = result.Error?.Description ?? "An error occurred",
            ErrorCode = result.Error?.Code,
            Data = default
        });
    }
}
