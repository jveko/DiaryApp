using Microsoft.AspNetCore.Mvc;

namespace DiaryApp.Responses;

public class InternalServerErrorResult : ObjectResult
{
    public InternalServerErrorResult(string message)
        : base(new {message})
    {
        StatusCode = StatusCodes.Status500InternalServerError;
    }
}