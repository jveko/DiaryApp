using AutoMapper;
using DiaryApp.Entities;
using DiaryApp.Interfaces;
using DiaryApp.Models;
using DiaryApp.Responses;
using DiaryApp.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace DiaryApp.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUserService _service;

    public UserController(IMapper mapper, IUserService service)
    {
        _mapper = mapper;
        _service = service;
    }

    private async Task<bool> IsEmailExist(string email)
    {
        var isValid = await _service.FindUserByEmail(email) != null;
        if (isValid) ModelState.AddModelError("Email", "The Email is already used");
        return isValid;
    }

    private async Task<bool> IsUserNameExist(string username)
    {
        var isValid = await _service.FindUserByUserName(username) != null;
        if (isValid) ModelState.AddModelError("UserName", "The UserName is already used");
        return isValid;
    }

    [HttpPost]
    [Route("signin")]
    public async Task<IActionResult> SignIn([FromBody] UserParamSignInModel userParamSignInModel)
    {
        try
        {
            var user = await _service.FindUserByEmail(userParamSignInModel.Email);
            if (user == null) return new NotFoundObjectResult(new {message = "Email is Not Registered"});
            if (!_service.IsPasswordValid(user.Password, userParamSignInModel.Password))
                return new AuthenticationErrorResult("Credentials is Wrong");
            var tokenModel = await _service.GenerateToken(user);
            HttpContext.Response.Cookies.Append("jwt", tokenModel.RefreshToken, new CookieOptions()
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                MaxAge = TimeSpan.FromHours(24)
            });
            return new OkObjectResult(
                new
                {
                    message = "Success",
                    data = new
                    {
                        accessToken = tokenModel.AccessToken,
                        refreshToken = tokenModel.RefreshToken,
                    }
                });
        }
        catch (Exception e)
        {
            return new InternalServerErrorResult("Failed to Sign In");
        }
    }

    [HttpPost]
    [Route("signup")]
    public async Task<IActionResult> SignUp([FromBody] UserParamSignUpModel userParamSignUpModel)
    {
        if(!userParamSignUpModel.PasswordMatched) return new BadRequestObjectResult(new
        {
            message = "Your Password is Not Matched with Password Confirmation"
        });
        if (await IsUserNameExist(userParamSignUpModel.Username) || await IsEmailExist(userParamSignUpModel.Email))
            return new ValidationFailedResult(ModelState);
        try
        {
            var user = _mapper.Map<User>(userParamSignUpModel);
            await _service.AddUser(user);
            return new ObjectCreatedResult(new {user.Id});
        }
        catch (Exception e)
        {
            return new InternalServerErrorResult("Failed to Create User");
        }
    }
}