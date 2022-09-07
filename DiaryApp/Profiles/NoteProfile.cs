using AutoMapper;
using DiaryApp.Entities;
using DiaryApp.Models;

namespace DiaryApp.Profiles;

public class NoteProfile : Profile
{
    public NoteProfile()
    {
        CreateMap<NoteParamCreateModel, Note>();
        CreateMap<Note, NoteDto>();
        CreateMap<User, UserInNoteDto>();
    }
}