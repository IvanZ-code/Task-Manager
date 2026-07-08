using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Task_Manager.DTOs.CommentDtos;
using Task_Manager.Interfaces;

namespace Task_Manager.Controllers;

[ApiController]
[Route("api/tasks/{taskId:int}/comments")]
[Authorize]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetComments(int taskId)
    {
        var comments = await _commentService.GetComments(
            taskId,
            GetUserId(),
            GetRole());

        return Ok(comments);
    }

    [HttpPost]
    public async Task<IActionResult> AddComment(
        int taskId,
        CreateCommentDto dto)
    {
        var comment = await _commentService.AddComment(
            taskId,
            dto,
            GetUserId(),
            GetRole());

        return Ok(comment);
    }

    [HttpPut("{commentId}")]
    public async Task<IActionResult> UpdateComment(
    int taskId,
    int commentId,
    UpdateCommentDto dto)
    {
        var result = await _commentService.UpdateComment(
            taskId,
            commentId,
            dto,
            GetUserId(),
            GetRole());

        if (!result)
            return BadRequest();

        return Ok(new
        {
            message = "Comment updated."
        });
    }

    [HttpDelete("{commentId}")]
    public async Task<IActionResult> DeleteComment(
    int taskId,
    int commentId)
    {
        var result = await _commentService.DeleteComment(
            taskId,
            commentId,
            GetUserId(),
            GetRole());

        if (!result)
            return BadRequest();

        return Ok(new
        {
            message = "Comment deleted."
        });
    }

    private int GetUserId()
    {
        return int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    private string GetRole()
    {
        return User.FindFirstValue(ClaimTypes.Role)!;
    }
}
