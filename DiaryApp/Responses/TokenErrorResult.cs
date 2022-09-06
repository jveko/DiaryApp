using Microsoft.AspNetCore.Mvc;

namespace DiaryApp.Responses;

public class TokenErrorResult : ObjectResult
{
    public TokenErrorResult(string message) : base(new {message})
    {
        StatusCode = StatusCodes.Status401Unauthorized;
    }
}