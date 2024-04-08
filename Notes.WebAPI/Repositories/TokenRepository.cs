using Microsoft.IdentityModel.Tokens;
using Notes.WebAPI.Models.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Notes.WebAPI.Repositories;

public class TokenRepository : ITokenRepository
{
    private readonly IConfiguration _configuration;

    public TokenRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string CreateJwtToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
           new("id",user.Id),
           new(ClaimTypes.Email, user.Email),
           new(ClaimTypes.NameIdentifier, user.Email),
           new(ClaimTypes.Role,"user")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"],claims,expires: DateTime.Now.AddDays(30), signingCredentials: credentials);

        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return token;
    }
}
