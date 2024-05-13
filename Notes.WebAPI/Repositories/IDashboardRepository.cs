using Notes.WebAPI.Models.Domain;

namespace Notes.WebAPI.Repositories;

public interface IDashboardRepository
{
    Task<ApiResponse<List<ApplicationUser>>> GetAllUsersAsync();
    Task<List<ApplicationUser>> GetUsersByRoleAsync(string role);
    Task<ApiResponse> DeleteUserAsync(string userId);
}
