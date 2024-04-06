using Microsoft.AspNetCore.Identity;

namespace Notes.WebAPI.Models.Domain;

public class ApplicationUser : IdentityUser
{
    public List<Note> Notes { get; set; } = [];
}
