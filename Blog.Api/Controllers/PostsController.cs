using Blog.Api.Mapping;
using Blog.Application.Services;
using Blog.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    [Authorize(AuthConstants.TrustMemberPolicyName)]
    [HttpPost(ApiEndpoints.Posts.Create)]
    public async Task<IActionResult> Create([FromBody] CreatePostRequest request,
        CancellationToken token)
    {
        var post = request.MapToPost();

        await _postService.CreateAsync(post, token);

        var response = post.MapToResponse();
        return CreatedAtAction(nameof(Get), new { idOrSlug = response.Id }, response);
    }

    [HttpGet(ApiEndpoints.Posts.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug,
        CancellationToken token)
    {
        var post = Guid.TryParse(idOrSlug, out var id)
            ? await _postService.GetByIdAsync(id, token)
            : await _postService.GetBySlugAsync(idOrSlug, token);

        if (post is null)
        {
            return NotFound();
        }

        var response = post.MapToResponse();
        return Ok(response);
    }

    [HttpGet(ApiEndpoints.Posts.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var posts = await _postService.GetAllAsync(token);

        var response = posts.MapToResponse();
        return Ok(response);
    }
    
    [Authorize(AuthConstants.TrustMemberPolicyName)]
    [HttpPut(ApiEndpoints.Posts.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id,
        [FromBody] UpdatePostRequest request, CancellationToken token)
    {
        var post = request.MapToPost(id);

        var updatedPost = await _postService.UpdateAsync(post, token);
        if (updatedPost is null)
        {
            return NotFound();
        }

        var response = updatedPost.MapToResponse();
        return Ok(response);
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpDelete(ApiEndpoints.Posts.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
    {
        var isDeleted = await _postService.DeleteByIdAsync(id, token);
        if (!isDeleted)
        {
            return NotFound();
        }

        return Ok();
    }
}