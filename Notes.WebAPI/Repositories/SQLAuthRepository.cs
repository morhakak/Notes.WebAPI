﻿using Microsoft.AspNetCore.Identity;
using Notes.WebAPI.Data;
using Notes.WebAPI.Models.Domain;
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

    public async Task<ApiResponse> Login(ApplicationUser user,string password)
    {
        if (user is null) 
        {
            return new ApiResponse
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "Invalid user"
            };
        }

        var existingUser = await _userManager.FindByEmailAsync(user.Email!);

        if (existingUser == null)
        {
            return new ApiResponse
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "Incorrect email or password",
            };
        }

        var result = await _userManager.CheckPasswordAsync(existingUser,password);

        if (!result)
        {
            return new ApiResponse
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "Incorrect email or password",
            };
        }

        var token = _tokenRepository.CreateJwtToken(existingUser);

        return new LoginResponse
        {
            Success = true,
            StatusCode = (int)HttpStatusCode.OK,
            Message = "User logged in successfully",
            Token = token
        };
    }

    public async Task<ApiResponse> Register(string email, string password)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);

        if (existingUser is not null)
        {
            return new ApiResponse
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "User already exists"
            };
        }

        var user = new ApplicationUser
        {
            Email = email,
            UserName = email,
            Roles = ["User"]
        };

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await _notesDbContext.SaveChangesAsync();
            return new ApiResponse
            {
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Message = "User registered successfully",
            };
        }

        return new ApiResponse
        {
            Success = false,
            StatusCode = (int)HttpStatusCode.InternalServerError,
            Message = $"Something went wrong, unable to register user: {result.Errors.FirstOrDefault()?.Description}",
        };
    }
}
