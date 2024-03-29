using Microsoft.EntityFrameworkCore;
using Notes.WebAPI.Data;
using Notes.WebAPI.Models.Domain;

namespace Notes.WebAPI.Repositories;

public class SQLNoteRepository : INoteRepository
{
    private readonly NotesDbContext _notesDbContext;

    public SQLNoteRepository(NotesDbContext notesDbContext)
    {
        _notesDbContext = notesDbContext;
    }

    public async Task<Note> CreateAsync(Note note)
    {
       await _notesDbContext.Notes.AddAsync(note);
        await _notesDbContext.SaveChangesAsync();
        return note;
    }

    public async Task<Note?> DeleteAsync(Guid id)
    {
        var note = await _notesDbContext.Notes.FindAsync(id);

        if (note == null)
        {
            return null;
        }
        
        _notesDbContext.Notes.Remove(note);
        await _notesDbContext.SaveChangesAsync();

        return note;
    }

    public async Task<List<Note>> GetAllAsync()
    {
       return await _notesDbContext.Notes.ToListAsync();
    }

    public async Task<Note?> GetByIdAsync(Guid id)
    {
        return await _notesDbContext.Notes.FindAsync(id);
    }

    public async Task<Note?> UpdateIsDoneAsync(Guid id, Note note)
    {
        var exsitingNote = await _notesDbContext.Notes.FindAsync(id);

        if (exsitingNote == null)
        {
            return null;
        }

        exsitingNote.IsDone = note.IsDone;

        await _notesDbContext.SaveChangesAsync();

        return exsitingNote;
    }

    public async Task<Note?> UpdateIsLikedAsync(Guid id, Note note)
    {
        var exsitingNote = await _notesDbContext.Notes.FindAsync(id);

        if (exsitingNote == null)
        {
            return null;
        }

        exsitingNote.IsLiked = note.IsLiked;

        await _notesDbContext.SaveChangesAsync();

        return exsitingNote;
    }
}
