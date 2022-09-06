using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DiaryApp.Entities;

[Index(nameof(UserName), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class User
{
    public User()
    {
    }

    [Key] public int Id { get; set; }

    [Required]
    [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required] 
    public string Password { get; set; }
    [Required] 
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public IList<Note> Notes { get; set; }
}

// public class UserDto
// {
//     [Key] public int Id { get; set; }
//     [Required] [MaxLength(50)] public string Username { get; set; }
//
//     [Required]
//     [MaxLength(50)]
//     [EmailAddress]
//     public string Email { get; set; }
// }