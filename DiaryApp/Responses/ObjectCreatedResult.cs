using DiaryApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DiaryApp.Responses;

public class ObjectCreatedResult : ObjectResult
{
    public ObjectCreatedResult(object obj) : base(new {message = "Success", data = obj})
    {
        StatusCode = StatusCodes.Status201Created;
    }
}