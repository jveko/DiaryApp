﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DiaryApp.Entities;

[Index(nameof(Title))]
public class Note
{
    public Note()
    {
        
    }
    [Key]
    public int Id { get; set; }
    [Required]
    [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
    public string Title { get; set; }
    [Required]
    [MaxLength(500)]
    public string Content { get; set; }
    [Required]
    [DefaultValue(false)]
    public bool IsArchive { get; set; }
    [Required]
    public int OwnerId { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    [ForeignKey("OwnerId")]
    public User User { get; set; }
}