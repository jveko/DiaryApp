using AutoMapper;
using DiaryApp.Entities;
using DiaryApp.Models;
using DiaryApp.Utilities;

namespace DiaryApp.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserParamPostModel, User>();
    }
}