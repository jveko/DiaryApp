using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DiaryApp.Models;

public class ValidationResultModel
{
    public ValidationResultModel(string message, ModelStateDictionary modelState)
    {
        Message = message;
        Errors = new Dictionary<string, List<string>>();
        foreach (var (key, value) in modelState)
        {
            Errors.Add(key.ToLower(), value.Errors.Select(r => r.ErrorMessage).ToList());
        }
    }

    public string Message { get; }
    public Dictionary<string, List<string>> Errors { get; }
}