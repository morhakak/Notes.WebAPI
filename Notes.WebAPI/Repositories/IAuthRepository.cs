using Notes.WebAPI.Models.Domain;

namespace Notes.WebAPI.Repositories;

public interface IAuthRepository
{
    Task<ApiResponse<string>> Register(ApplicationUser user, string password);
    Task<ApiResponse<string>> Login(ApplicationUser user, string password);
}
