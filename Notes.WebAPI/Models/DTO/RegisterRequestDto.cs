using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Notes.WebAPI.Models.DTO;

public class RegisterRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\\W_]).{6,}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, one special character, and be at least 6 characters long.")]
    public string Password { get; set; }
}
