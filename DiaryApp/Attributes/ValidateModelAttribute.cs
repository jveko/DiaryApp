using DiaryApp.Responses;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DiaryApp.Attributes;

public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            context.Result = new ValidationErrorResult(context.ModelState);
        }
    }
}