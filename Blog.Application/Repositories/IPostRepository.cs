using Blog.Application.Models;

namespace Blog.Application.Repositories;

public interface IPostRepository
{
    Task<bool> CreateAsync(Post post);
    
    Task<Post?> GetByIdAsync(Guid id);

    Task<Post?> GetBySlugAsync(string slug);

    Task<IEnumerable<Post>> GetAllAsync();

    Task<bool> UpdateAsync(Post post);

    Task<bool> DeleteByIdAsync(Guid id);

    Task<bool> ExistsByIdAsync(Guid id);
}