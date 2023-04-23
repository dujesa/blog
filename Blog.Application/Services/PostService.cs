using Blog.Application.Models;
using Blog.Application.Repositories;
using FluentValidation;

namespace Blog.Application.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IValidator<Post> _postValidator;
    private readonly IRatingRepository _ratingRepository;
    private readonly IValidator<GetAllPostsOptions> _optionsValidator;

    public PostService(IPostRepository postRepository, IValidator<Post> postValidator, IRatingRepository ratingRepository, IValidator<GetAllPostsOptions> optionsValidator)
    {
        _postRepository = postRepository;
        _postValidator = postValidator;
        _ratingRepository = ratingRepository;
        _optionsValidator = optionsValidator;
    }

    public async Task<bool> CreateAsync(Post post, CancellationToken token = default)
    { 
        await _postValidator.ValidateAndThrowAsync(post, cancellationToken: token);
        return await _postRepository.CreateAsync(post, token);
    }

    public Task<Post?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default)
    {
        return _postRepository.GetByIdAsync(id, userId, token);
    }

    public Task<Post?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken token = default)
    {
        return _postRepository.GetBySlugAsync(slug, userId, token);
    }

    public async Task<IEnumerable<Post>> GetAllAsync(GetAllPostsOptions options, CancellationToken token = default)
    {
        await _optionsValidator.ValidateAndThrowAsync(options, token);
        
        return await _postRepository.GetAllAsync(options, token);
    }

    public async Task<Post?> UpdateAsync(Post post, Guid? userId = default, CancellationToken token = default)
    {
        await _postValidator.ValidateAndThrowAsync(post, cancellationToken: token);
        var postExists = await _postRepository.ExistsByIdAsync(post.Id, token);
        if (!postExists)
        {
            return null;
        }

        await _postRepository.UpdateAsync(post, userId, token);

        if (!userId.HasValue)
        {
            var rating = await _ratingRepository.GetRatingAsync(post.Id, token);
            post.Rating = rating;

            return post;
        }

        var (postRating, userRating) = await _ratingRepository.GetRatingAsync(post.Id, userId.Value, token);
        post.Rating = postRating;
        post.UserRating = userRating;
        
        return post;
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        return _postRepository.DeleteByIdAsync(id, token);
    }
}