namespace DiaryApp.Responses;

public class PaginatedModel<TEntity>
{
    public PaginatedModel(int totalCount, int filteredCount, IReadOnlyList<TEntity> pageData)
    {
        TotalCount = totalCount;
        FilteredCount = filteredCount;
        PageData = pageData;
    }

    public int TotalCount { get; set; }
    public int FilteredCount { get; set; }
    public IReadOnlyList<TEntity> PageData { get; set; }
}