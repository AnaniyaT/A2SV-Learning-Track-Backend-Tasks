using System.Linq.Expressions;
using Api.Data;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly CommentService _commentService;

    public CommentsController(BlogDbContext context)
    {
        _commentService = new CommentService(context);
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_commentService.GetAll());
    }
    
    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        Comment? comment = _commentService.GetById(id);

        if (comment is null)
            return NotFound("Comment does not exist");

        return Ok(comment);
    }

    [HttpGet("/PostId/{postId:int}")]
    public IActionResult GetByPostId(int postId)
    {
        return Ok(_commentService.GetByPostId(postId));
    }

    [HttpPost]
    public IActionResult Create(CommentContent content)
    {
        try
        {
            Comment comment = _commentService.Create(new Comment() { PostId = content.PostId, Text = content.Text });
            comment.Post = null;
            return CreatedAtAction("Get", comment.Id, comment);
        }
        catch (InvalidOperationException)
        {
            return BadRequest("Post does not exist");
        }
        catch (Exception)
        {
            return StatusCode(500, "Server Error");
        }
    }

    [HttpPatch]
    public IActionResult Patch(int id, CommentContent contents)
    {
        try
        {
            var comment = _commentService.Update(id, contents.Text);
            return Ok(comment);
        }
        catch (InvalidOperationException)
        {
            return BadRequest("Comment does not exist");
        }
        catch (Exception)
        {
            return StatusCode(500, "Server Error");
        }
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        try
        {
            _commentService.Delete(id);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return BadRequest("Comment does not exist");
        }
        catch (Exception)
        {
            return StatusCode(500, "Server Error");
        }
    }
}