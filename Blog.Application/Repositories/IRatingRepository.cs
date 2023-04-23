using Blog.Application.Models;

namespace Blog.Application.Repositories;

public interface IRatingRepository
{
    Task<bool> RatePostAsync(Guid postId, int rating, Guid userId, CancellationToken token = default);
    Task<float?> GetRatingAsync(Guid postId, CancellationToken token = default);
    Task<(float? Rating, int? UserRating)> GetRatingAsync(Guid postId, Guid userId, CancellationToken token = default);
    Task<bool> DeleteRatingAsync(Guid postId, Guid userId, CancellationToken token = default);
    Task<IEnumerable<PostRating>> GetRatingsForUserAsync(Guid userId, CancellationToken token = default);
}