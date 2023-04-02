namespace Blog.Contracts.Responses;

public class PostsResponse
{
    public IEnumerable<PostResponse> Posts { get; init; } = Enumerable.Empty<PostResponse>();
}