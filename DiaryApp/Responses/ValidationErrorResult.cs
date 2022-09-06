using DiaryApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DiaryApp.Responses;

public class ValidationErrorResult : ObjectResult
{
    public ValidationErrorResult(ModelStateDictionary modelState)
        : base(new ValidationResultModel("Validation Error", modelState))
    {
        StatusCode = StatusCodes.Status400BadRequest;
    }
}