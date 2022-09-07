using System.Linq.Expressions;
using DiaryApp.Entities;
using DiaryApp.Models;
using DiaryApp.Responses;

namespace DiaryApp.Interfaces;

public interface INoteService
{
    public Task AddNote(Note note, int ownerId);
    public Task<Note?> GetNote(int id, IEnumerable<Expression<Func<Note, object>>>? includes = null);
    // public Task<Note?> GetNote(int id, Expression<Func<Note, object>>? includes);
    public Task UpdateNote(Note note);
    public Task ArchiveNote(Note note);
    public Task<PaginatedModel<Note>> GetPagedNote(PaginatedParamModel<int> paramModel, int ownerId);
}