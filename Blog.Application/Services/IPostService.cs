using Blog.Application.Models;

namespace Blog.Application.Services;

public interface IPostService
{
    Task<bool> CreateAsync(Post post, CancellationToken token = default);

    Task<Post?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default);

    Task<Post?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken token = default);

    Task<IEnumerable<Post>> GetAllAsync(GetAllPostsOptions options, CancellationToken token = default);

    Task<Post?> UpdateAsync(Post post, Guid? userId = default, CancellationToken token = default);

    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
}