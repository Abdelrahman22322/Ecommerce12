using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    // POST: api/Comments
    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddComment(CreateCommentDto createCommentDto)
    {
        var comment = await _commentService.AddCommentAsync(createCommentDto);
        return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, comment);
    }

    // GET: api/Comments/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetCommentById(int id)
    {
        var comment = await _commentService.GetCommentByIdAsync(id);
        if (comment == null)
        {
            return NotFound();
        }
        return Ok(comment);
    }

    // GET: api/Comments/Product/{productId}
    [HttpGet("Product/{productId}")]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsByProductId(int productId)
    {
        var comments = await _commentService.GetCommentsByProductIdAsync(productId);
        return Ok(comments);
    }

    // PUT: api/Comments/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<CommentDto>> UpdateComment(int id, UpdateCommentDto updateCommentDto)
    {
        var updatedComment = await _commentService.UpdateCommentAsync(id, updateCommentDto);
        if (updatedComment == null)
        {
            return NotFound();
        }
        return Ok(updatedComment);
    }

    // DELETE: api/Comments/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteComment(int id)
    {
        var result = await _commentService.DeleteCommentAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}