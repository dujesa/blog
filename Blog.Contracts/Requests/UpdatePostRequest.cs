namespace Blog.Contracts.Requests;

public class UpdatePostRequest
{
    public string Title { get; init; }
    
    public DateTime CreatedAt { get; init; } 
    
    public IEnumerable<string> Categories { get; init; } = Enumerable.Empty<string>();
}