using System.Linq.Expressions;
using DiaryApp.Contexts;
using DiaryApp.Entities;
using DiaryApp.Interfaces;
using DiaryApp.Models;
using DiaryApp.Responses;
using Microsoft.EntityFrameworkCore;

namespace DiaryApp.Services;

public class NoteService : INoteService
{
    private readonly DiaryContext _context;

    public NoteService(DiaryContext context)
    {
        _context = context;
    }

    public async Task AddNote(Note note, int ownerId)
    {
        note.CreatedAt = DateTime.Now;
        note.OwnerId = ownerId;
        note.IsArchive = false;
        await _context.Notes.AddAsync(note);
        await _context.SaveChangesAsync();
    }

    public async Task<Note?> GetNote(int id, IEnumerable<Expression<Func<Note, object>>>? includes = null)
    {
        var notes = _context.Notes.AsQueryable();
        if (includes != null)
        {
            notes = includes.Aggregate(notes, (current, include) => current.Include(include));
        }
        var note = await notes.FirstOrDefaultAsync(v => v.Id.Equals(id));
        return note;
    }

    public async Task UpdateNote(Note note, NoteParamPutModel model)
    {
        note.Title = model.Title;
        note.Content = model.Content;
        note.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
    }

    public async Task ArchiveNote(Note note)
    {
        note.IsArchive = !note.IsArchive;
        await _context.SaveChangesAsync();
    }

    public async Task<PaginatedModel<Note>> GetPagedNote(PaginatedParamModel<int> paramModel, int ownerId)
    {
        var notes = _context.Notes.AsQueryable();
        notes = notes.Where(v => v.OwnerId == ownerId && !v.IsArchive);
        var totalCount = notes.Count();
        var filteredCount = totalCount;

        if (paramModel.Q != null)
        {
            notes = notes.Where(v => v.Title.Contains(paramModel.Q));
            filteredCount = notes.Count();
        }

        notes = paramModel.Increment ? notes.Where(v => v.Id > paramModel.Id) : notes.Where(v => v.Id < paramModel.Id);
        notes = notes.Take(paramModel.Size);
        return new PaginatedModel<Note>(totalCount: totalCount, filteredCount: filteredCount,
            pageData: await notes.ToListAsync());
    }
}