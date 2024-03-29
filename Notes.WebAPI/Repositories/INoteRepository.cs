using Notes.WebAPI.Models.Domain;

namespace Notes.WebAPI.Repositories
{
    public interface INoteRepository
    {
       Task<List<Note>> GetAllAsync();
       Task<Note?> GetByIdAsync(Guid id);
        Task<Note> CreateAsync(Note note);
        Task<Note?> UpdateIsLikedAsync(Guid id,Note note);
        Task<Note?> UpdateIsDoneAsync(Guid id,Note note);
        Task<Note?> DeleteAsync(Guid id);
    }
}
