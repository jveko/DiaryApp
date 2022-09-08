using AutoMapper;
using DiaryApp.Entities;
using DiaryApp.Interfaces;
using DiaryApp.Models;
using DiaryApp.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DiaryApp.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUserService _service;

    public UserController(IMapper mapper, IUserService service)
    {
        _mapper = mapper;
        _service = service;
    }

    private static IActionResult PasswordConfirmationNotMatch() => new BadRequestObjectResult(new
    {
        message = "Your Password is Not Matched with Password Confirmation"
    });

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
    [Route("")]
    public async Task<IActionResult> PostUser([FromBody] UserParamPostModel model)
    {
        if (!model.PasswordMatched) return PasswordConfirmationNotMatch();
        if (await IsUserNameExist(model.Username) || await IsEmailExist(model.Email))
            return new ValidationFailedResult(ModelState);
        var user = _mapper.Map<User>(model);
        await _service.AddUser(user);
        return new ObjectCreatedResult(new {user.Id});
    }
}