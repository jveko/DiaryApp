using DiaryApp.Exceptions;
using DiaryApp.Interfaces;
using DiaryApp.Responses;
using DiaryApp.Utilities;
using Newtonsoft.Json;

namespace DiaryApp.Middlewares;

public class ValidateTokenMiddleware
{
    private readonly RequestDelegate _next;

    public ValidateTokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUserService userService, ITokenService tokenService)
    {
        try
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault();
            if (token == null) throw new AuthorizationInvalidException("Token Empty");
            token = token.Split(" ").Last();
            if (string.IsNullOrEmpty(token)) throw new AuthorizationInvalidException("Wrong Format Token");
            var claims = tokenService.ValidateAccessToken(token);
            if (claims == null) throw new AuthorizationInvalidException("Token Expired");
            if (!claims.HasClaim(r => r.Type.Equals("userId")))
                throw new AuthorizationInvalidException("Token Invalid");
            var id = claims.Claims.First(r => r.Type.Equals("userId"));
            if (!int.TryParse(id.Value, out var realId)) throw new AuthorizationInvalidException("Token Invalid");
            context.Items["User"] = await userService.GetUser(realId);
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        if (ex is AuthorizationInvalidException)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                message = ex.Message
            }));
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                message = ex.Message
            }));
        }
    }
}