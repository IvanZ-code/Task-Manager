using Microsoft.EntityFrameworkCore;
using System;
using Task_Manager.Data;
using Task_Manager.DTOs.HistoryDtos;
using Task_Manager.Enums;
using Task_Manager.Interfaces;
using Task_Manager.Models;

namespace Task_Manager.Services;

public class HistoryService : IHistoryService
{
    private readonly DataContext _context;

    public HistoryService(DataContext context)
    {
        _context = context;
    }

    public async Task AddRecord(
        int taskId,
        int userId,
        HistoryAction action,
        string? description = null,
        string? metadata = null)
    {
        var history = new TaskHistory
        {
            TaskId = taskId,
            UserId = userId,
            Action = action,
            Description = description,
            Metadata = metadata,
            CreatedAt = DateTime.UtcNow
        };

        _context.TaskHistories.Add(history);

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<HistoryDto>> GetHistory(
    int taskId)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == taskId);

        if (task == null)
            throw new KeyNotFoundException("Task not found.");

        return await _context.TaskHistories
            .Include(h => h.User)
            .Where(h => h.TaskId == taskId)
            .OrderBy(h => h.CreatedAt)
            .Select(h => new HistoryDto
            {
                Id = h.Id,
                CreatedAt = h.CreatedAt,
                User = h.User.Login,
                Action = h.Action,
                Description = h.Description,
                Metadata = h.Metadata
            })
            .ToListAsync();
    }
}
