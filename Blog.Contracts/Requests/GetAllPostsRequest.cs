namespace Blog.Contracts.Requests;

public class GetAllPostsRequest
{
    public string? Title { get; set; }
    public int? Year { get; set; }
    public required string? SortBy { get; init; }
}