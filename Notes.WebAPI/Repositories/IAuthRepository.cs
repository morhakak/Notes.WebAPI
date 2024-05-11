using Notes.WebAPI.Models.Domain;
using Notes.WebAPI.Models.DTO;

namespace Notes.WebAPI.Repositories;

public interface IAuthRepository
{
    Task<ApiResponse> Register(string email, string password);
    Task<ApiResponse> Login(ApplicationUser user, string password);
}
