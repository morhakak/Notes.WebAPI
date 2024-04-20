using System.ComponentModel.DataAnnotations;

namespace Notes.WebAPI.Models.DTO;

public class LoginRequestDto
{
    [Required]
    [EmailAddress(ErrorMessage = "Please enter a valid email")]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}
