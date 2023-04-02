using Blog.Application.Models;

namespace Blog.Application.Services;

public interface IPostService
{
    Task<bool> CreateAsync(Post post);

    Task<Post?> GetByIdAsync(Guid id);

    Task<Post?> GetBySlugAsync(string slug);

    Task<IEnumerable<Post>> GetAllAsync();

    Task<Post?> UpdateAsync(Post post);

    Task<bool> DeleteByIdAsync(Guid id);
}