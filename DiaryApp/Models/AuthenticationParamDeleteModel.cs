using System.ComponentModel.DataAnnotations;

namespace DiaryApp.Models;

public class AuthenticationParamDeleteModel
{
    public string RefreshToken { get; set; }
}