using Microsoft.AspNetCore.Mvc;
namespace DiaryApp.Responses;

public class AuthenticationErrorResult: ObjectResult
{
    public AuthenticationErrorResult(string message) : base(new {message})
    {
        StatusCode = StatusCodes.Status401Unauthorized;
    }
}