using DiaryApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiaryApp.Contexts;

public sealed class DiaryContext : DbContext
{
    public DiaryContext(DbContextOptions<DiaryContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Authentication> Authentications => Set<Authentication>();
    public DbSet<Note> Notes => Set<Note>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Authentication>().ToTable("Authentications");
        modelBuilder.Entity<Note>().ToTable("Notes");
    }
}