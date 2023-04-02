using Blog.Api.Mapping;
using Blog.Application.Repositories;
using Blog.Application.Services;
using Blog.Contracts.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
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

    [HttpPost(ApiEndpoints.Posts.Create)]
    public async Task<IActionResult> Create([FromBody] CreatePostRequest request)
    {
        var post = request.MapToPost();
        
        await _postService.CreateAsync(post);
        
        var response = post.MapToResponse();
        return CreatedAtAction(nameof(Get), new { idOrSlug = response.Id }, response);
    }

    [HttpGet(ApiEndpoints.Posts.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug)
    {
        var post = Guid.TryParse(idOrSlug, out var id)
            ? await _postService.GetByIdAsync(id)
            : await _postService.GetBySlugAsync(idOrSlug);
            
        if (post is null)
        {
            return NotFound();
        }

        var response = post.MapToResponse();
        return Ok(response);
    }

    [HttpGet(ApiEndpoints.Posts.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var posts = await _postService.GetAllAsync();

        var response = posts.MapToResponse();
        return Ok(response);
    }

    [HttpPut(ApiEndpoints.Posts.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id,
        [FromBody] UpdatePostRequest request)
    {
        var post = request.MapToPost(id);
        
        var updatedPost = await _postService.UpdateAsync(post);
        if (updatedPost is null)
        {
            return NotFound();
        }

        var response = updatedPost.MapToResponse();
        return Ok(response);
    }

    [HttpDelete(ApiEndpoints.Posts.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var isDeleted = await _postService.DeleteByIdAsync(id);
        if (!isDeleted)
        {
            return NotFound();
        }

        return Ok();
    }
}