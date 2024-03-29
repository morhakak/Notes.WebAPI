using AutoMapper;
using Notes.WebAPI.Models.Domain;
using Notes.WebAPI.Models.DTO;

namespace Notes.WebAPI.Mapping
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Note,NoteDto>().ReverseMap();
            CreateMap<CreateNoteDto,Note>().ReverseMap();
            CreateMap<UpdateNoteRequestDto, Note>().ReverseMap();
            CreateMap<UpdateIsLikedRequestDto, Note>().ReverseMap();
            CreateMap<UpdateIsDoneRequestDto, Note>().ReverseMap();
        }
    }
}
