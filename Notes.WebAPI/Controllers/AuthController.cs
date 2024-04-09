using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.WebAPI.Models.Domain;
using Notes.WebAPI.Models.DTO;
using Notes.WebAPI.Repositories;

namespace Notes.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _authRepository;

    public AuthController(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        var response = await _authRepository.Register(registerRequestDto, registerRequestDto.Password);

        if (response.Success)
        {
            return Ok(response.Message);
        }

        return BadRequest(response.Message);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var appUser = new ApplicationUser
        {
            Email = loginRequestDto.Email,
        };

        var response = await _authRepository.Login(appUser, loginRequestDto.Password);

        if (response.Success)
        {
            return Ok(response.Message); 
        }

        return BadRequest(response.Message);
    }
}
