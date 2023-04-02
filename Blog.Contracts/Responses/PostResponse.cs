namespace Blog.Contracts.Responses;

public class PostResponse
{
    public Guid Id { get; init; }
    
    public string Title { get; init; }

    public string Slug { get; init; }
    
    public DateTime CreatedAt { get; init; }

    public IEnumerable<string> Categories { get; init; } = Enumerable.Empty<string>();
}