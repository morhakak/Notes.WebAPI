using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notes.WebAPI.Data;
using Notes.WebAPI.Models.Domain;
using Notes.WebAPI.Models.DTO;
using Notes.WebAPI.Repositories;

namespace Notes.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotesController : ControllerBase
{
    private readonly NotesDbContext _dbContext;
    private readonly INoteRepository _noteRepository;
    private readonly IMapper _mapper;

    public NotesController(NotesDbContext dbContext, INoteRepository noteRepository, IMapper mapper)
    {
        _dbContext = dbContext;
        _noteRepository = noteRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var notesDomain = await _noteRepository.GetAllAsync();

        var notesDto = _mapper.Map<List<NoteDto>>(notesDomain);

        return Ok(notesDto);
    }

    [HttpGet("{id:Guid}")]
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
    public async Task<IActionResult> Create([FromBody] CreateNoteDto createNoteDto)
    {
       var noteDomain =  _mapper.Map<Note>(createNoteDto);

       noteDomain =  await _noteRepository.CreateAsync(noteDomain);

        var noteDto = _mapper.Map<NoteDto>(noteDomain);

        return CreatedAtAction(nameof(GetById), new { id = noteDomain.Id }, noteDto);
    }

    //[HttpPut("{id:Guid}")]
    //public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateNoteRequestDto updateNoteRequestDto)
    //{
    //    var noteDomain = _mapper.Map<Note>(updateNoteRequestDto);

    //    noteDomain = await _noteRepository.UpdateAsync(id, noteDomain);

    //    if (noteDomain == null)
    //    {
    //        return NotFound();
    //    }

    //    var noteDto = _mapper.Map<NoteDto>(noteDomain);

    //    return Ok(noteDto);
    //}

    [HttpPut("{id:Guid}/isLiked")]
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
