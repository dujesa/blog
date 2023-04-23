namespace Blog.Contracts.Responses;

public class PostRatingResponse
{
    public required Guid PostId { get; init; }
    
    public required string Slug { get; init; }

    public required int Rating { get; init; }
}