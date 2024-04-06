using Notes.WebAPI.Models.Domain;

namespace Notes.WebAPI.Repositories;

public interface ITokenRepository
{
    string CreateJwtToken(ApplicationUser user);
}
