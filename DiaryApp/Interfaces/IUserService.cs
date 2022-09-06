using DiaryApp.Entities;
using DiaryApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DiaryApp.Interfaces;

public interface IUserService
{
    Task<User> GetUser(int id);
    Task<User?> FindUserByEmail(string email);
    Task<User?> FindUserByUserName(string userName);
    bool IsPasswordValid(string hashPassword, string plainPassword);
    Task AddUser(User user);
    Task<TokenModel> GenerateToken(User user);
}