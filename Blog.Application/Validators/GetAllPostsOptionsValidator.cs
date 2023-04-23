using Blog.Application.Models;
using FluentValidation;

namespace Blog.Application.Validators;

public class GetAllPostsOptionsValidator : AbstractValidator<GetAllPostsOptions>
{
    private static readonly string[] AcceptableSortFields =
    {
        "title", "createdat"
    };
    
    public GetAllPostsOptionsValidator()
    {
        RuleFor(x => x.YearOfPublish)
            .LessThanOrEqualTo(DateTime.UtcNow.Year);

        RuleFor(x => x.SortField)
            .Must(x => x is null || AcceptableSortFields.Contains(x, StringComparer.OrdinalIgnoreCase))
            .WithMessage("You can only sort by 'title' or 'createdat'!");
    }
}