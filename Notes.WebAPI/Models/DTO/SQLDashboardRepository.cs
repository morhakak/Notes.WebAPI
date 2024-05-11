using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Notes.WebAPI.Data;
using Notes.WebAPI.Models.Domain;
using Notes.WebAPI.Repositories;

namespace Notes.WebAPI.Models.DTO;

public class SQLDashboardRepository : IDashboardRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly NotesDbContext _dbContext;

    public SQLDashboardRepository(UserManager<ApplicationUser> userManager, NotesDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }
    public async Task<List<ApplicationUser>> GetAllUsers()
    {
        var users = await _userManager.Users.ToListAsync();

        return users;
    }

    public async Task<List<ApplicationUser>> GetUsersByRole(string roleName)
    {
        var usersInRole = await _dbContext.Users
            .Where(u => u.Roles.Contains(roleName))
            .ToListAsync();

        return usersInRole;
    }
}
