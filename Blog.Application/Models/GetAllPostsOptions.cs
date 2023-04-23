namespace Blog.Application.Models;

public class GetAllPostsOptions
{
    public string? Title { get; set; }
    public int? YearOfPublish { get; set; }
    public Guid? UserId { get; set; }
    public string? SortField { get; set; }
    public SortOrder? SortOrder { get; set; }
}

public enum SortOrder
{
    Unsorted,
    Ascending,
    Descending
}