using Application.Commons.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace API.Extensions
{
    /// <summary>
    ///     Extensions for transforming <see cref="Application.Commons.Results.Result{T}"/>
    ///     to <see cref="ObjectResult"/>
    /// </summary>
    public static class MapResultToActionResult
    {
        /// <summary>
        ///     Return the http status code from an error <see cref="Error"/> object.
        /// </summary>
        /// <param name="error">The <see cref="Error"/> object</param>
        /// <returns></returns>
        public static int MapErrorToStatusCode(Error error)
        {
            return error.Code switch
            {
                Error.Invalid => StatusCodes.Status400BadRequest,
                Error.Unauthenticated => StatusCodes.Status401Unauthorized,
                Error.Unauthorized => StatusCodes.Status403Forbidden,
                Error.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError,
            };
        }

        /// <summary>
        ///     Map a <see cref="Application.Commons.Results.Result{T}"/> to a <see cref="ObjectResult"/>.
        /// </summary>
        /// <typeparam name="T">The data type of the result</typeparam>
        /// <param name="result">The activity result</param>
        /// <param name="body_type">The Content-Type string that will be put in the header</param>
        /// <returns>An <see cref="IActionResult"/> object</returns>
        public static IActionResult MapToResult<T>(this Result<T> result, string body_type)
        {
            var resultObject = new ObjectResult(result)
            {
                ContentTypes = new MediaTypeCollection { body_type },
            };

            if (result.IsSuccess)
            {
                resultObject.StatusCode = StatusCodes.Status200OK;
                return resultObject;
            }

            resultObject.StatusCode = MapErrorToStatusCode(result.Error!);
            return resultObject;
        }


        /// <summary>
        ///     Map a <see cref="Application.Commons.Results.Result{T}"/> to a <see cref="ObjectResult"/> with 
        ///     body type of json.
        /// </summary>
        /// <typeparam name="T">The data type of the result</typeparam>
        /// <param name="result">The activity result</param>
        /// <returns>An <see cref="IActionResult"/> object</returns>
        public static IActionResult MapToJsonResult<T>(this Result<T> result)
        {
            return MapToResult<T>(result, "application/json");
        }
    }
}
