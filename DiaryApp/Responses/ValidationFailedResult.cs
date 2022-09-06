using DiaryApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DiaryApp.Responses;

public class ValidationFailedResult : ObjectResult
{
    public ValidationFailedResult(ModelStateDictionary modelState)
        : base(new ValidationResultModel("Validation Failed", modelState))
    {
        StatusCode = StatusCodes.Status422UnprocessableEntity;
    }
}