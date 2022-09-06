using DiaryApp.Interfaces;
using DiaryApp.Utilities;

namespace DiaryApp.Middlewares;

public class ValidateTokenMiddleware
{
    private readonly RequestDelegate _next;

    public ValidateTokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUserService userService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault();
        if (token != null)
        {
            token = token.Split(" ").Last();
            var claims = TokenManager.ValidateAccessToken(token);
            var id = claims?.Claims.FirstOrDefault(r => r.Type.Equals("userId"));
            if (id != null)
            {
                if (int.TryParse(id.Value, out var realId))
                {
                    context.Items["User"] = await userService.GetUser(realId);
                }
            }
        }


        await _next(context);
    }
}