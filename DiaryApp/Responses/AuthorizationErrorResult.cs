using Microsoft.AspNetCore.Mvc;

namespace DiaryApp.Responses;

public class AuthorizationErrorResult : ObjectResult
{
    public AuthorizationErrorResult(string message) : base(new {message})
    {
        StatusCode = StatusCodes.Status401Unauthorized;
    }
}