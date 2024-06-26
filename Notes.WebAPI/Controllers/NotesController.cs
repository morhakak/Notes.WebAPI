﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes.WebAPI.CustomActionFilters;
using Notes.WebAPI.CustomAttributes;
using Notes.WebAPI.Data;
using Notes.WebAPI.Models.Domain;
using Notes.WebAPI.Models.DTO;
using Notes.WebAPI.Repositories;
using System.Security.Claims;

namespace Notes.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]

public class NotesController : ControllerBase
{
    private readonly INoteRepository _noteRepository;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly NotesDbContext _notesDbContext;

    public NotesController(INoteRepository noteRepository, IMapper mapper, UserManager<ApplicationUser> userManager, NotesDbContext dbContext)
    {
        _noteRepository = noteRepository;
        _mapper = mapper;
        _userManager = userManager;
        _notesDbContext = dbContext;
    }

    [HttpGet]
    [Authorize(Policy = "AppUser")]
    public async Task<IActionResult> GetAllByUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }

        var notesDomain = await _noteRepository.GetNotesByUserIdAsync(userId);

        var notesDto = _mapper.Map<List<NoteDto>>(notesDomain);

        return Ok(notesDto);
    }

    [HttpGet("admin")]
    [Authorize(Policy = "AppAdmin")] 
    public async Task<IActionResult> GetAllForAdmin()
    {
        var notesDomain = await _noteRepository.GetAllAsync();

        var notesDto = _mapper.Map<List<NoteDto>>(notesDomain);

        return Ok(notesDto);
    }

    [HttpGet("{id:Guid}")]
    [Authorize(Policy = "AppUser")]
    [Authorize(Policy = "AppAdmin")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var noteDomain = await _noteRepository.GetByIdAsync(id);

        if (noteDomain == null)
        {
            return NotFound();
        }

        var noteDto = _mapper.Map<NoteDto>(noteDomain);

        return Ok(noteDto);
    }

    [HttpPost]
    [ValidateModel]
    [RolesAuthorize("User", "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateNoteDto createNoteDto)
    {
       var noteDomain =  _mapper.Map<Note>(createNoteDto);

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var user = await _notesDbContext.Users.FindAsync(userId);

        if (user == null)
        {
            return Unauthorized();
        }

        noteDomain.UserId = user.Id;

        noteDomain =  await _noteRepository.CreateAsync(noteDomain);

        var noteDto = _mapper.Map<NoteDto>(noteDomain);

        return CreatedAtAction(nameof(GetById), new { id = noteDomain.Id }, noteDto);
    }

    [HttpPut("{id:Guid}/isLiked")]
    [RolesAuthorize("User", "Admin")]
    public async Task<IActionResult> UpdateIsLiked([FromRoute] Guid id, [FromBody] UpdateIsLikedRequestDto updateIsLikedRequestDto)
    {
        var noteDomain = _mapper.Map<Note>(updateIsLikedRequestDto);

        noteDomain = await _noteRepository.UpdateIsLikedAsync(id, noteDomain);

        if (noteDomain == null)
        {
            return NotFound();
        }

        var noteDto = _mapper.Map<NoteDto>(noteDomain);

        return Ok(noteDto);
    }

    [HttpPut("{id:Guid}/isDone")]
    [RolesAuthorize("User", "Admin")]
    public async Task<IActionResult> UpdateIsDone([FromRoute] Guid id, [FromBody] UpdateIsDoneRequestDto updateIsDoneRequestDto)
    {
        var noteDomain = _mapper.Map<Note>(updateIsDoneRequestDto);

        noteDomain = await _noteRepository.UpdateIsDoneAsync(id, noteDomain);

        if (noteDomain == null)
        {
            return NotFound();
        }

        var noteDto = _mapper.Map<NoteDto>(noteDomain);

        return Ok(noteDto);
    }

    [HttpDelete("{id:Guid}")]
    [RolesAuthorize("User", "Admin")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var noteDomain = await _noteRepository.DeleteAsync(id);

        if (noteDomain == null)
        {
            return NotFound();
        }

        var noteDto = _mapper.Map<NoteDto>(noteDomain);

        return Ok(noteDto);
    }
}
