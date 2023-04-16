using Blog.Application.Models;
using Blog.Application.Repositories;
using FluentValidation;

namespace Blog.Application.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IValidator<Post> _postValidator;

    public PostService(IPostRepository postRepository, IValidator<Post> postValidator)
    {
        _postRepository = postRepository;
        _postValidator = postValidator;
    }

    public async Task<bool> CreateAsync(Post post, CancellationToken token = default)
    { 
        await _postValidator.ValidateAndThrowAsync(post, cancellationToken: token);
        return await _postRepository.CreateAsync(post, token);
    }

    public Task<Post?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return _postRepository.GetByIdAsync(id, token);
    }

    public Task<Post?> GetBySlugAsync(string slug, CancellationToken token = default)
    {
        return _postRepository.GetBySlugAsync(slug, token);
    }

    public Task<IEnumerable<Post>> GetAllAsync(CancellationToken token = default)
    {
        return _postRepository.GetAllAsync(token);
    }

    public async Task<Post?> UpdateAsync(Post post, CancellationToken token = default)
    {
        await _postValidator.ValidateAndThrowAsync(post, cancellationToken: token);
        var postExists = await _postRepository.ExistsByIdAsync(post.Id, token);
        if (!postExists)
        {
            return null;
        }

        await _postRepository.UpdateAsync(post, token);
        return post;
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        return _postRepository.DeleteByIdAsync(id, token);
    }
}