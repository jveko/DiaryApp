using Microsoft.AspNetCore.Mvc;

namespace DiaryApp.Responses;

public class InternalServerErrorResult : ObjectResult
{
    public InternalServerErrorResult()
        : base(new {message = "Sorry, there was a failure on our server."})
    {
        StatusCode = StatusCodes.Status500InternalServerError;
    }
}