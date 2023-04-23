using System.Text.RegularExpressions;

namespace Blog.Application.Models;

public class Post
{
    public Guid Id { get; init; }
    
    public string Title { get; set; }
    
    public string Slug => GenerateSlug();

    public float? Rating { get; set; }
    
    public int? UserRating { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public List<string> Categories { get; init; } = new();
    
    private string GenerateSlug()
    {
        var sluggedTitle = Regex
            .Replace(Title, "[^0-9A-Za-z _-]", string.Empty)
            .ToLower()
            .Replace(" ", "-");

        var sluggedCreatedAt = DateOnly.FromDateTime(CreatedAt)
            .ToString()
            .Replace("/", "-");
        
        return $"{sluggedTitle}-{sluggedCreatedAt}";
    }
}