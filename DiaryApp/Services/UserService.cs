using DiaryApp.Contexts;
using DiaryApp.Entities;
using DiaryApp.Interfaces;
using DiaryApp.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DiaryApp.Services;

public class UserService : IUserService
{
    private readonly DiaryContext _context;

    public UserService(DiaryContext context)
    {
        _context = context;
    }

    private string HashPassword(string plainPassword)
    {
        return BCrypt.HashPassword(plainPassword, BCrypt.GenerateSalt());
    }

    public async Task<User> GetUser(int id)
    {
        return await _context.Users.FirstAsync(u => u.Id.Equals(id));
    }

    public async Task<User?> FindUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Email.Equals(email));
    }

    public async Task<User?> FindUserByUserName(string userName)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.UserName.Equals(userName));
    }

    public async Task AddUser(User user)
    {
        user.Password = HashPassword(user.Password);
        user.CreatedAt = DateTime.Now;
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}