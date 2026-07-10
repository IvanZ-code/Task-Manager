using Microsoft.EntityFrameworkCore;
using System;
using Task_Manager.Data;
using Task_Manager.DTOs.CommentDtos;
using Task_Manager.Interfaces;
using Task_Manager.Models;

namespace Task_Manager.Services;

public class CommentService : ICommentService
{
    private readonly DataContext _context;

    public CommentService(DataContext context)
    {
        _context = context;
    }
    private async Task<TaskItem> GetAccessibleTask(
    int taskId,
    int userId,
    string role)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == taskId);

        if (task == null)
            throw new KeyNotFoundException("Task not found");

        var user = await _context.Users
            .FirstOrDefaultAsync(t => t.Id == userId);

        if (user == null)
            throw new KeyNotFoundException("User not found");

        if (role != "Admin" &&
            task.CreatorId != userId &&
            task.ExecutorId != userId)
        {
            throw new UnauthorizedAccessException();
        }

        return task;
    }

    public async Task<CommentDto> AddComment(
    int taskId,
    CreateCommentDto dto,
    int userId,
    string role)
    {
        var task = await GetAccessibleTask(
            taskId,
            userId,
            role);

        var comment = new Comment
        {
            Text = dto.Text,
            TaskItemId = task.Id,
            AuthorId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Comments.Add(comment);

        await _context.SaveChangesAsync();

        await _context.Entry(comment)
            .Reference(c => c.Author)
            .LoadAsync();

        return new CommentDto
        {
            Id = comment.Id,
            Author = comment.Author.Login,
            Text = comment.Text,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt
        };
    }

    public async Task<IEnumerable<CommentDto>> GetComments(
    int taskId,
    int userId,
    string role)
    {
        var task = await GetAccessibleTask(
            taskId,
            userId,
            role);

        return await _context.Comments
            .Where(c =>
                c.TaskItemId == task.Id &&
                !c.IsDeleted)
            .OrderBy(c => c.CreatedAt)
            .Select(c => new CommentDto
            {
                Id = c.Id,
                Author = c.Author.Login,
                Text = c.Text,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            })
            .ToListAsync();
    }


    public async Task<bool> UpdateComment(
    int taskId,
    int commentId,
    UpdateCommentDto dto,
    int userId,
    string role)
    {
        var comment = await _context.Comments
            .FirstOrDefaultAsync(c =>
                c.Id == commentId &&
                !c.IsDeleted);

        if (comment == null)
            throw new KeyNotFoundException("Comment not found.");

        if (comment.TaskItemId != taskId)
            throw new KeyNotFoundException("Comment does not belong to the specified task.");

        await GetAccessibleTask(
            comment.TaskItemId,
            userId,
            role);

        if (role != "Admin" &&
            comment.AuthorId != userId)
        {
            throw new UnauthorizedAccessException();
        }

        comment.Text = dto.Text;
        comment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }


    public async Task<bool> DeleteComment(
    int taskId,
    int commentId,
    int userId,
    string role)
    {
        var comment = await _context.Comments
            .FirstOrDefaultAsync(c =>
                c.Id == commentId &&
                !c.IsDeleted);

        if (comment == null)
            throw new KeyNotFoundException("Comment not found.");

        if (comment.TaskItemId != taskId)
            throw new KeyNotFoundException("Comment does not belong to the specified task.");

        await GetAccessibleTask(
            comment.TaskItemId,
            userId,
            role);

        if (role != "Admin" &&
            comment.AuthorId != userId)
        {
            throw new UnauthorizedAccessException();
        }

        comment.IsDeleted = true;
        comment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }
}
