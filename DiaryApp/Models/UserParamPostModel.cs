using System.ComponentModel.DataAnnotations;

namespace DiaryApp.Models;

public class UserParamPostModel
{
    [Required] 
    [MaxLength(50)] 
    [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
    public string Username { get; set; }

    [Required]
    [MaxLength(50)]
    [EmailAddress]
    public string Email { get; set; }

    [Required] 
    public string Password { get; set; }
    [Required]
    public string PasswordConfirmation { get; set; }

    public bool PasswordMatched => Password.Equals(PasswordConfirmation);
}