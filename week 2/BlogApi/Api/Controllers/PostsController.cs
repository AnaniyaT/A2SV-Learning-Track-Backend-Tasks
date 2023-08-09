using Api.Data;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly PostService _postService;
    
    public PostsController(BlogDbContext context)
    {
        _postService = new PostService(context);
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_postService.GetAll());
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        Post? post = _postService.GetById(id);

        if (post is null)
            return NotFound("Post Not Found");

        return Ok(post);
    }

    [HttpPost]
    public IActionResult Post(PostContents postContents)
    {
        try
        {
            if (postContents.Title.Length < 1)
                return BadRequest("Title can't be empty");
            if (postContents.Content.Length < 1)
                return BadRequest("Content can't be empty");

            var createdPost = _postService.Create(new Post() {Title = postContents.Title, Content = postContents.Content});
            return CreatedAtAction("Get", createdPost.Id, createdPost);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Server Error");
        }
    }

    [HttpPatch]
    public IActionResult Patch(int id, PostContents postContents)
    {
        try
        {
            string? newTitle = null;
            string? newContent = null;
            if (postContents.Title.Length >= 1)
                newTitle = postContents.Title;
            if (postContents.Content.Length >= 1)
                newContent = postContents.Content;

            var updatedPost = _postService.Update(id, newTitle, newContent);

            return Ok(updatedPost);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest("Post does not exist");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Server Error");
        }
    }
    
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        try
        {
            _postService.Delete(id);

            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return BadRequest("Post does not exist");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Server Error");
        }
    }
}
