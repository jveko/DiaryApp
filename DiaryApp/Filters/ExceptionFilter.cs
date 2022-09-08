using DiaryApp.Responses;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DiaryApp.Filters;

public class ExceptionFilter: IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        context.Result = new InternalServerErrorResult();
    }
}