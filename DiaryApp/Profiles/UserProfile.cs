using AutoMapper;
using DiaryApp.Entities;
using DiaryApp.Models;

namespace DiaryApp.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserParamPostModel, User>();
    }
}