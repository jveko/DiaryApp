using DiaryApp.Interfaces;
using DiaryApp.Models;
using DiaryApp.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DiaryApp.Controllers;

[ApiController]
[Route("authentications")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _service;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public AuthenticationController(IAuthenticationService service, IUserService userService,
        ITokenService tokenService)
    {
        _service = service;
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> PostAuthentication([FromBody] AuthenticationParamPostModel model)
    {
        var user = await _userService.FindUserByEmail(model.Email);
        if (user == null) return new NotFoundObjectResult(new {message = "Email is Not Registered"});
        if (!_service.IsPasswordValid(user.Password, model.Password))
            return new AuthenticationErrorResult("Credentials is Wrong");
        var accessToken = _tokenService.GenerateAccessToken(user.Id);
        var refreshToken = _tokenService.GenerateRefreshToken(user.Id);
        await _service.AddRefreshToken(refreshToken);
        return new OkObjectResult(
            new
            {
                message = "Success",
                data = new
                {
                    accessToken,
                    refreshToken
                }
            });
    }

    [HttpPut]
    [Route("")]
    public async Task<IActionResult> PutAuthentication([FromBody]AuthenticationParamPutModel model)
    {
        var authentication = await _service.GetRefreshToken(model.RefreshToken);
        if (authentication == null) return new AuthorizationErrorResult("Token Invalid");
        var claims = _tokenService.ValidateRefreshToken(model.RefreshToken);
        if (claims == null || !claims.HasClaim(r => r.Type.Equals("userId")))
            return new AuthorizationErrorResult("Token Invalid");
        var userId = claims.Claims.First(r => r.Type.Equals("userId")).Value;
        var accessToken = _tokenService.GenerateAccessToken(int.Parse(userId));
        return new OkObjectResult(
            new
            {
                status = "success",
                message = "Access Token is updated",
                data = new
                {
                    accessToken
                }
            });
    }

    [HttpDelete]
    [Route("")]
    public async Task<IActionResult> DeleteAuthentication([FromBody]AuthenticationParamDeleteModel model)
    {
        var authentication = await _service.GetRefreshToken(model.RefreshToken);
        if (authentication == null) return new AuthorizationErrorResult("Token Invalid");
        await _service.DeleteRefreshToken(authentication);
        return new OkObjectResult(
            new
            {
                status = "success",
                message= "Refresh token is removed"
            });
    }
}