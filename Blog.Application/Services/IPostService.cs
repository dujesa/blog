using Blog.Application.Models;

namespace Blog.Application.Services;

public interface IPostService
{
    Task<bool> CreateAsync(Post post, CancellationToken token = default);

    Task<Post?> GetByIdAsync(Guid id, CancellationToken token = default);

    Task<Post?> GetBySlugAsync(string slug, CancellationToken token = default);

    Task<IEnumerable<Post>> GetAllAsync(CancellationToken token = default);

    Task<Post?> UpdateAsync(Post post, CancellationToken token = default);

    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
}