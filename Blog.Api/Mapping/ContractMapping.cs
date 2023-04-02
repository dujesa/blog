using Blog.Application.Models;
using Blog.Contracts.Requests;
using Blog.Contracts.Responses;

namespace Blog.Api.Mapping;

public static class ContractMapping
{
    public static Post MapToPost(this CreatePostRequest request)
    {
        return new Post
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Categories = request.Categories.ToList(),
            CreatedAt = request.CreatedAt,
        };
    }
    
    public static Post MapToPost(this UpdatePostRequest request, Guid id)
    {
        return new Post
        {
            Id = id,
            Title = request.Title,
            Categories = request.Categories.ToList(),
            CreatedAt = request.CreatedAt,
        };
    }

    public static PostResponse MapToResponse(this Post post)
    {
        return new PostResponse
        {
            Id = post.Id,
            Title = post.Title,
            Slug = post.Slug,
            Categories = post.Categories,
            CreatedAt = post.CreatedAt
        };
    }

    public static PostsResponse MapToResponse(this IEnumerable<Post> posts)
    {
        return new PostsResponse
        {
            Posts = posts.Select(MapToResponse)
        };
    }
}