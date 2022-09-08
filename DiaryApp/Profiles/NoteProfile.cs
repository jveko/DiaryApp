using AutoMapper;
using DiaryApp.Entities;
using DiaryApp.Models;

namespace DiaryApp.Profiles;

public class NoteProfile : Profile
{
    public NoteProfile()
    {
        CreateMap<NoteParamPostModel, Note>();
        CreateMap<NoteParamPutModel, Note>();
        CreateMap<Note, NoteGetByIdDto>();
        CreateMap<User, UserInNoteGetByIdDto>();
        CreateMap<Note, NoteGetAllDto>();
        CreateMap<Note, NotePutByIdDto>();
    }
}