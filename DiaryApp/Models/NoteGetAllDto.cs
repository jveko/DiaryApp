using System.ComponentModel.DataAnnotations;

namespace DiaryApp.Models;

public class NoteGetAllDto
{
    public int Id { get; set; }
    [Required]
    [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
    public string Title { get; set; }
    [Required]
    [MaxLength(500)]
    public string Content { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}