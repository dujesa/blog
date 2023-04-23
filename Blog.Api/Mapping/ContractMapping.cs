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
            Rating = post.Rating,
            UserRating = post.UserRating,
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
    
    public static IEnumerable<PostRatingResponse> MapToResponse(this IEnumerable<PostRating> ratings)
    {
        return ratings.Select(x => new PostRatingResponse
        {
            Rating = x.Rating,
            Slug = x.Slug,
            PostId = x.PostId,
        });
    }

    public static GetAllPostsOptions MapToOptions(this GetAllPostsRequest request)
    {
        return new GetAllPostsOptions
        {
            Title = request.Title,
            YearOfPublish = request.Year,
            SortField = request.SortBy?.Trim('+', '-'),
            SortOrder = request.SortBy is null ? SortOrder.Unsorted :
                request.SortBy.StartsWith('-') ? SortOrder.Descending : SortOrder.Ascending
        };
    }

    public static GetAllPostsOptions WithUser(this GetAllPostsOptions options,
        Guid? userId)
    {
        options.UserId = userId;

        return options;
    }
}