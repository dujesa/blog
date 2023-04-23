namespace Blog.Application.Models;

public class PostRating
{
    public required Guid PostId { get; init; }
    
    public required string Slug { get; init; }

    public required int Rating { get; init; }
}