using Domain.DomainResults;
using Microsoft.AspNetCore.Mvc;

public static class ResultExtensions
{
    //2xx results
    public static IActionResult ToOkResult<T>(this ResultData<T> result)
    {
        return new OkObjectResult(result);
    }
    public static IActionResult ToCreateResult<T>(this ResultData<T> data, string path)
    {
        return new CreatedResult(path, data);
    }
    public static IActionResult ToNoContentResult<T>(this ResultData<T> _)
    {
        return new NoContentResult();
    }
    //Esse resultado genérico quando a API precisa retornar um único dado, que no caso de ser nulo, já retorna o NotFound.
    public static IActionResult ToSingleResult<T>(this ResultData<T> result)
    {
        return result.IsSuccess ? new OkObjectResult(result) : new NotFoundObjectResult(result);
    }
    //4xx results
    public static IActionResult ToBadRequestResult<T>(this ResultData<T> result)
    {
        return new BadRequestObjectResult(result);
    }
    public static IActionResult ToUnauthorizedResult<T>(this ResultData<T> result)
    {
        return new UnauthorizedObjectResult(result);
    }
    public static IActionResult ToConflictResult<T>(this T _, string message)
    {
        return new ConflictObjectResult(ResultData<T>.Error(message));
    }
    public static IActionResult ToNotFoundResult<T>(this ResultData<T> result)
    {
        return new NotFoundObjectResult(result);
    }
    public static IActionResult ToNotFoundResultGeneric<T>(this T? _, string message)
    {
        return new NotFoundObjectResult(message);
    }
}