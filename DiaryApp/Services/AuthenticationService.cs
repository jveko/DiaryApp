using DiaryApp.Contexts;
using DiaryApp.Entities;
using DiaryApp.Interfaces;
using DiaryApp.Models;
using DiaryApp.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DiaryApp.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly ITokenService _tokenService;
    private readonly DiaryContext _context;

    public AuthenticationService(ITokenService tokenService, DiaryContext context)
    {
        _tokenService = tokenService;
        _context = context;
    }
    
    public bool IsPasswordValid(string hashPassword, string plainPassword)
    {
        return BCrypt.CheckPassword(plainPassword, hashPassword);
    }

    public async Task AddRefreshToken(string refreshToken)
    {
        await _context.Authentications.AddAsync(new Authentication(refreshToken));
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRefreshToken(Authentication authentication)
    {
        _context.Authentications.Remove(authentication);
        await _context.SaveChangesAsync();
    }

    public async Task<Authentication?> GetRefreshToken(string refreshToken)
    {
        return await _context.Authentications.FirstOrDefaultAsync(r => r.Token.Equals(refreshToken));
    }
}