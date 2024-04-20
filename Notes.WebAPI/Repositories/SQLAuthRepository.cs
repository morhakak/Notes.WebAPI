using Microsoft.AspNetCore.Identity;
using Notes.WebAPI.Data;
using Notes.WebAPI.Models.Domain;
using Notes.WebAPI.Models.DTO;
using System.Net;

namespace Notes.WebAPI.Repositories;

public class SQLAuthRepository : IAuthRepository
{
    private readonly NotesDbContext _notesDbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenRepository _tokenRepository;

    public SQLAuthRepository(NotesDbContext dbContext, UserManager<ApplicationUser> userManager, ITokenRepository tokenRepository)
    {
        _notesDbContext = dbContext;
        _userManager = userManager;
        _tokenRepository = tokenRepository;
    }

    public async Task<ApiResponse<string>> Login(ApplicationUser user,string password)
    {
        if (user is null) 
        {
            return new ApiResponse<string>
            {
                Success = false,
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Invalid user"
            };
        }

        var existingUser = await _userManager.FindByEmailAsync(user.Email!);

        if (existingUser == null)
        {
            return new ApiResponse<string>
            {
                Success = false,
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Incorrect email or password",
            };
        }

        var result = await _userManager.CheckPasswordAsync(existingUser,password);

        if (!result)
        {
            return new ApiResponse<string>
            {
                Success = false,
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Incorrect email or password",
            };
        }

        var token = _tokenRepository.CreateJwtToken(existingUser);

        return new ApiResponse<string>
        {
            Success = true,
            StatusCode = HttpStatusCode.OK,
            Message = "User logged in successfully",
            Data = token
        };
    }

    public async Task<ApiResponse<string>> Register(RegisterRequestDto userDto, string password)
    {
        var existingUser = _userManager.FindByEmailAsync(userDto.Email);

        if (existingUser is not null)
        {
            return new ApiResponse<string>
            {
                Success = false,
                StatusCode = HttpStatusCode.BadRequest,
                Message = "User already exists"
            };
        }

        var user = new ApplicationUser
        {
            Email = userDto.Email,
            UserName = userDto.Email
        };

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await _notesDbContext.SaveChangesAsync();
            return new ApiResponse<string>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Message = "User registered successfully",
            };
        }

        return new ApiResponse<string>
        {
            Success = false,
            StatusCode = HttpStatusCode.InternalServerError,
            Message = $"Something went wrong, unable to register user: {result.Errors.FirstOrDefault()?.Description}",
        };
    }
}
