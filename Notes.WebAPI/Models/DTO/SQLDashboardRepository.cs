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

    public async Task<ApiResponse> DeleteUserAsync(string userId)
    {
        var user = await _dbContext.Users.FindAsync(userId);

        if(user == null)
        {
            return new ApiResponse
            {
                StatusCode = (int)StatusCodes.Status404NotFound,
                Message = "User not found",
                Success = false
            };
        }

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            return new ApiResponse
            {
                StatusCode = (int)StatusCodes.Status500InternalServerError,
                Message = "Failed to delete user",
                Success = false
            };
        }

        return new ApiResponse
        {
            StatusCode = (int)StatusCodes.Status204NoContent,
            Message = "User was deleted successfully",
            Success = true
        };
    }

    public async Task<ApiResponse<List<ApplicationUser>>> GetAllUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();

        return new ApiResponse<List<ApplicationUser>>
        {
            Data = users,
            StatusCode = StatusCodes.Status200OK,
            Message = string.Empty,
            Success = true
        };
    }

    public async Task<List<ApplicationUser>> GetUsersByRoleAsync(string roleName)
    {
        var usersInRole = await _dbContext.Users
            .Where(u => u.Roles.Contains(roleName))
            .ToListAsync();

        return usersInRole;
    }
}
