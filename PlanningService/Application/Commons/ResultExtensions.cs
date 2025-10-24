using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Application.Commons
{
    public static class ResultExtensions
    {
        // Cho Task<Result>
        public static async Task<IActionResult> ToActionResult(this Task<Result> resultTask)
        {
            var result = await resultTask;
            return result.ToActionResult();
        }

        // Cho Task<Result<T>>
        public static async Task<IActionResult> ToActionResult<T>(this Task<Result<T>> resultTask)
        {
            var result = await resultTask;
            return result.ToActionResult();
        }

        // Cho Result
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

        // Cho Result<T>
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
}
