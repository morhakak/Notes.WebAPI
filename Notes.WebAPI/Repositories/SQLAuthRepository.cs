using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Notes.WebAPI.Data;
using Notes.WebAPI.Models.Domain;
using Notes.WebAPI.Models.DTO;

namespace Notes.WebAPI.Repositories;

public class SQLAuthRepository : IAuthRepository
{
    private readonly NotesDbContext _notesDbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenRepository _tokenRepository;
    private readonly IMapper _mapper;

    public SQLAuthRepository(NotesDbContext dbContext, UserManager<ApplicationUser> userManager, ITokenRepository tokenRepository, IMapper mapper)
    {
        _notesDbContext = dbContext;
        _userManager = userManager;
        _tokenRepository = tokenRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<string>> Login(ApplicationUser user,string password)
    {
        var existingUser =  await _userManager.FindByEmailAsync(user.Email);

        if (existingUser == null)
        {
            return new ApiResponse<string>
            {
                Success = false,
                Message = "Incorrect email or password",
                Data = null
            };
        }

        var result = await _userManager.CheckPasswordAsync(existingUser,password);

        if (!result)
        {
            return new ApiResponse<string>
            {
                Success = false,
                Message = "Incorrect email or password",
                Data = null
            };
        }

        var token = _tokenRepository.CreateJwtToken(existingUser);

        return new ApiResponse<string>
        {
            Success = true,
            Message = "User logged in successfully",
            Data = token
        };
    }

    public async Task<ApiResponse<string>> Register(RegisterRequestDto userDto, string password)
    {
        var existingUser = _userManager.FindByEmailAsync(userDto.Email);

        if (existingUser is null)
        {
            return new ApiResponse<string>
            {
                Data = null,
                Success = false,
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
                Message = "User registered successfully",
                Data = null
            };
        }

        return new ApiResponse<string>
        {
            Success = false,
            Message = $"Something went wrong, unable to register user: {result.Errors.FirstOrDefault()?.Description}",
            Data = null
        };
    }
}
