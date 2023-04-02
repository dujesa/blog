namespace Blog.Contracts.Requests;

public class CreatePostRequest
{
    public string Title { get; init; }

    public DateTime CreatedAt { get; init; } = DateTime.Now;

    public IEnumerable<string> Categories { get; init; } = Enumerable.Empty<string>();
}