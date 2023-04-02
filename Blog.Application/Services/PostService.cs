using Blog.Application.Models;
using Blog.Application.Repositories;

namespace Blog.Application.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;

    public PostService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public Task<bool> CreateAsync(Post post)
    {
        return _postRepository.CreateAsync(post);
    }

    public Task<Post?> GetByIdAsync(Guid id)
    {
        return _postRepository.GetByIdAsync(id);
    }

    public Task<Post?> GetBySlugAsync(string slug)
    {
        return _postRepository.GetBySlugAsync(slug);
    }

    public Task<IEnumerable<Post>> GetAllAsync()
    {
        return _postRepository.GetAllAsync();
    }

    public async Task<Post?> UpdateAsync(Post post)
    {
        var postExists = await _postRepository.ExistsByIdAsync(post.Id);
        if (!postExists)
        {
            return null;
        }

        await _postRepository.UpdateAsync(post);
        return post;
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        return _postRepository.DeleteByIdAsync(id);
    }
}