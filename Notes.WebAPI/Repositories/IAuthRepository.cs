using Notes.WebAPI.Models.Domain;
using Notes.WebAPI.Models.DTO;

namespace Notes.WebAPI.Repositories;

public interface IAuthRepository
{
    Task<ApiResponse<string>> Register(RegisterRequestDto user, string password);
    Task<ApiResponse<string>> Login(ApplicationUser user, string password);
}
