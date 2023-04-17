using Blog.Application.Models;
using Blog.Application.Repositories;
using FluentValidation;

namespace Blog.Application.Validators;

public class PostValidator : AbstractValidator<Post>
{
    private readonly IPostRepository _postRepository;
    
    public PostValidator(IPostRepository postRepository)
    {
        _postRepository = postRepository;
        
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Categories)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty();

        RuleFor(x => x.CreatedAt)
            .LessThanOrEqualTo(DateTime.UtcNow);

        RuleFor(x => x.Slug)
            .MustAsync(ValidateSlug)
            .WithMessage("This post already exists in the system");
    }

    private async Task<bool> ValidateSlug(Post post, string slug, CancellationToken token)
    {
        var existingPost = await _postRepository.GetBySlugAsync(slug);

        if (existingPost is null)
            return true;

        return existingPost.Id == post.Id;
    }
}