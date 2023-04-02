using Blog.Application.Database;
using Blog.Application.Models;
using Dapper;

namespace Blog.Application.Repositories;

public class PostRepository : IPostRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public PostRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> CreateAsync(Post post)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition(@"
            insert into posts (id, slug, title, createdat)
            values (@Id, @Slug, @Title, @CreatedAt)
        ", post));

        if (result < 0)
        {
            return false;
        }

        foreach (var category in post.Categories)
        {
            await connection.ExecuteAsync(new CommandDefinition(@"
                insert into categories (postId, name)
                values (@PostId, @Name)
            ", new { PostId = post.Id, Name = category }));
        }

        transaction.Commit();

        return result > 0;
    }

    public async Task<Post?> GetByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var post = await connection.QuerySingleOrDefaultAsync<Post>(
            new CommandDefinition(@"
                select * from posts where id = @id
            ", new { id }));

        if (post is null)
        {
            return null;
        }

        var categories = await connection.QueryAsync<string>(
            new CommandDefinition(@"
                select name from categories where postId = @id
            ", new { id }));

        foreach (var category in categories)
        {
            post.Categories.Add(category);
        }

        return post;
    }

    public async Task<Post?> GetBySlugAsync(string slug)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var post = await connection.QuerySingleOrDefaultAsync<Post>(
            new CommandDefinition(@"
                select * from posts where slug = @slug
            ", new { slug }));

        if (post is null)
        {
            return null;
        }

        var categories = await connection.QueryAsync<string>(
            new CommandDefinition(@"
                select name from categories where postId = @id
            ", new { id = post.Id }));

        foreach (var category in categories)
        {
            post.Categories.Add(category);
        }

        return post;
    }

    public async Task<IEnumerable<Post>> GetAllAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var result = await connection.QueryAsync(new CommandDefinition(@"
            select p.*, string_agg(c.name, ',') as categories
            from posts p left join categories c on p.id = c.postId
            group by id
        "));

        return result.Select(x => new Post
        {
            Id = x.id,
            Title = x.title,
            CreatedAt = x.createdat,
            Categories = Enumerable.ToList(x.categories.Split(','))
        });
    }

    public async Task<bool> UpdateAsync(Post post)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(new CommandDefinition(@"
            delete from categories where postid = @id
        ", new { id = post.Id }));

        foreach (var category in post.Categories)
        {
            await connection.ExecuteAsync(new CommandDefinition(@"
                insert into categories (postId, name)
                values (@PostId, @Name)
            ", new { PostId = post.Id, Name = category }));
        }

        var result = await connection.ExecuteAsync(new CommandDefinition(@"
            update posts set slug = @Slug, title = @Title, createdat = @CreatedAt
            where id = @Id
        ", post));

        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(new CommandDefinition(@"
            delete from categories where postid = @id
        ", new { id }));

        var result = await connection.ExecuteAsync(new CommandDefinition(@"
            delete from posts where id = @id
        ", new { id }));
        
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(@"
            select count(1) from posts where id = @id
        ", new { id }));
    }
}