using Microsoft.EntityFrameworkCore;
using Notes.WebAPI.Models.Domain;

namespace Notes.WebAPI.Data;

public class NotesDbContext : DbContext
{
    public NotesDbContext(DbContextOptions options) : base(options) {}

    public DbSet<Note> Notes { get; set; }
    public DbSet<ApplicationUser> Users { get; set; }
}
