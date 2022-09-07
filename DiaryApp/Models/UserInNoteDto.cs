using System.ComponentModel.DataAnnotations;

namespace DiaryApp.Models;

public class UserInNoteDto
{
    [Key] public int Id { get; set; }
    [Required]
    [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
    public string UserName { get; set; }

    [Required] [EmailAddress] public string Email { get; set; }
}