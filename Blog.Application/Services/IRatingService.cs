using Blog.Application.Models;

namespace Blog.Application.Services;

public interface IRatingService
{
    Task<bool> RatePostAsync(Guid postId, int rating, Guid userId, CancellationToken token = default);
    Task<bool> DeleteRatingAsync(Guid postId, Guid userId, CancellationToken token = default);
    Task<IEnumerable<PostRating>> GetRatingsForUserAsync(Guid userId, CancellationToken token = default);
}