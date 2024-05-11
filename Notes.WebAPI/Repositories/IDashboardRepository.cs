using Notes.WebAPI.Models.Domain;

namespace Notes.WebAPI.Repositories;

public interface IDashboardRepository
{
    Task<List<ApplicationUser>> GetAllUsers();
    Task<List<ApplicationUser>> GetUsersByRole(string role);
}
