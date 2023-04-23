using Blog.Application.Models;
using Blog.Application.Repositories;
using FluentValidation;
using FluentValidation.Results;

namespace Blog.Application.Services;

public class RatingService : IRatingService
{
    private readonly IRatingRepository _ratingRepository;
    private readonly IPostRepository _postRepository;

    public RatingService(IRatingRepository ratingRepository, IPostRepository postRepository)
    {
        _ratingRepository = ratingRepository;
        _postRepository = postRepository;
    }

    public async Task<bool> RatePostAsync(Guid postId, int rating, Guid userId, CancellationToken token = default)
    {
        if (rating is <= 0 or > 5)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure
                {
                    PropertyName = "Rating",
                    ErrorMessage = "Rating must be between 1 and 5."
                }
            });
        }

        var postExists = await _postRepository.ExistsByIdAsync(postId, token);
        if (!postExists)
        {
            return false;
        }

        return await _ratingRepository.RatePostAsync(postId, rating, userId, token);
    }

    public Task<bool> DeleteRatingAsync(Guid postId, Guid userId, CancellationToken token = default)
    {
        return _ratingRepository.DeleteRatingAsync(postId, userId, token);
    }

    public Task<IEnumerable<PostRating>> GetRatingsForUserAsync(Guid userId, CancellationToken token = default)
    {
        return _ratingRepository.GetRatingsForUserAsync(userId, token);
    }
}