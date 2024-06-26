﻿using Microsoft.AspNetCore.Mvc;
using Notes.WebAPI.CustomActionFilters;
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

    [HttpPost("Register")]
    [ValidateModel]
    public async Task<ActionResult<ApiResponse>> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        var response = await _authRepository.Register(registerRequestDto.Email, registerRequestDto.Password);

        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }

    [HttpPost("Login")]
    [ValidateModel]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var appUser = new ApplicationUser
        {
            Email = loginRequestDto.Email,
        };

        var response = await _authRepository.Login(appUser, loginRequestDto.Password);

        if (response.Success)
        {
            return Ok(response); 
        }

        return BadRequest(response);
    }
}
