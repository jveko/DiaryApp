﻿using System.ComponentModel.DataAnnotations;

namespace DiaryApp.Models;

public class UserParamSignInModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}