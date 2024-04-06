using System.ComponentModel.DataAnnotations;

namespace Notes.WebAPI.Models.DTO
{
    public class CreateNoteDto
    {
        [Required]
        [MinLength(1,ErrorMessage = "Note title should contains at least 1 character")]
        [MaxLength(50,ErrorMessage = "Note title has to be a maximum of 50 characters")]
        public string Title { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "Note content should contains at least 1 character")]
        [MaxLength(100, ErrorMessage = "Note content has to be a maximum of 50 characters")]
        public string Content { get; set; }
    }
}
