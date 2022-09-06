using System.ComponentModel.DataAnnotations;

namespace DiaryApp.Entities;

public class Authentication
{
    public Authentication()
    {
    }

    public Authentication(string token)
    {
        Token = token;
    }

    [Key] public string Token { get; set; }
}