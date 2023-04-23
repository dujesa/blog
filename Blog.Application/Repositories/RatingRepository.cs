using Blog.Application.Database;
using Blog.Application.Models;
using Dapper;

namespace Blog.Application.Repositories;

public class RatingRepository : IRatingRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public RatingRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> RatePostAsync(Guid postId, int rating, Guid userId, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        var result = await connection.ExecuteAsync(new CommandDefinition(@"
            insert into ratings(userid, postid, rating)
            values (@userId, @postId, @rating)
            on conflict (userId, postId) do update
                set rating = @rating
        ", new { userId, postId, rating }, cancellationToken: token));

        return result > 0;
    }

    public async Task<float?> GetRatingAsync(Guid postId, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        return await connection.QuerySingleOrDefaultAsync<float?>(new CommandDefinition(@"
            select round(avg(r.rating), 1) from ratings r
            where postid = @postId
        ", new { postId }, cancellationToken: token));
    }

    public async Task<(float? Rating, int? UserRating)> GetRatingAsync(Guid postId, Guid userId, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        return await connection.QuerySingleOrDefaultAsync<(float?, int?)>(new CommandDefinition(@"
            select round(avg(r.rating), 1),
                   (select rating 
                    from ratings 
                    where postid = @postId 
                      and userid = @userId 
                    limit 1)
            from ratings
            where postid = @postId
        ", new { postId, userId }, cancellationToken: token));
    }

    public async Task<bool> DeleteRatingAsync(Guid postId, Guid userId, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        var result = await connection.ExecuteAsync(new CommandDefinition(@"
            delete from ratings
            where postid = @postId
            and userid = @userId
        ", new { userId, postId }, cancellationToken: token));

        return result > 0;
    }

    public async Task<IEnumerable<PostRating>> GetRatingsForUserAsync(Guid userId, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        return await connection.QueryAsync<PostRating>(new CommandDefinition(@"
            select r.rating, r.postid, p.slug
            from ratings r
            inner join posts p on r.postid = p.id
            where userid = @userId
        ", new { userId }, cancellationToken: token));
    }
}