using System.ComponentModel.DataAnnotations;

namespace DiaryApp.Models;

public class NoteParamPostModel
{
    [Required]
    [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
    public string Title { get; set; }
    [Required]
    [MaxLength(500)]
    public string Content { get; set; }
}