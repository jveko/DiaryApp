using System.Linq.Expressions;

namespace DiaryApp.Models;

public class PaginatedParamModel<T>
{
    private int _size = 10;

    public PaginatedParamModel()
    {
        
    }
    public PaginatedParamModel(int size, bool increment, T id)
    {
        Size = size;
        Increment = increment;
        Id = id;
    }
    public PaginatedParamModel(int size, bool increment, T id, string? q)
    {
        Size = size;
        Increment = increment;
        Id = id;
        Q = q;
    }

    public int Size
    {
        get => _size > 100 ? 100 : _size;
        set => _size = value;
    }

    public bool Increment { get; set; } = true;
    public string? Q { get; set; }
    public T Id { get; set; }
}