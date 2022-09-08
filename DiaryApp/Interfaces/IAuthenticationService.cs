using DiaryApp.Entities;
using DiaryApp.Models;

namespace DiaryApp.Interfaces;

public interface IAuthenticationService
{
    bool IsPasswordValid(string hashPassword, string plainPassword);
    Task AddRefreshToken(string refreshToken);
    Task DeleteRefreshToken(Authentication authentication);
    Task<Authentication?> GetRefreshToken(string refreshToken);
}