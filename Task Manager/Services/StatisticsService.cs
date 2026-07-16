using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Task_Manager.Data;
using Task_Manager.DTOs.StatisticsDtos;
using Task_Manager.Enums;
using Task_Manager.Interfaces;

namespace Task_Manager.Services;

public class StatisticsService : IStatisticsService
{
    private readonly DataContext _context;

    public StatisticsService(DataContext context)
    {
        _context = context;
    }

    public async Task<StatisticsDto> GetStatistics()
    {
        return new StatisticsDto
        {
            TotalUsers = await _context.Users.CountAsync(),

            Admins = await _context.Users.CountAsync(u => u.Role == UserRole.Admin),

            Managers = await _context.Users.CountAsync(u => u.Role == UserRole.Manager),

            Employees = await _context.Users.CountAsync(u => u.Role == UserRole.Employee),

            TotalTasks = await _context.Tasks.CountAsync(),

            NewTasks = await _context.Tasks.CountAsync(t => t.Status == TaskState.New),

            InProgressTasks = await _context.Tasks.CountAsync(t => t.Status == TaskState.InProgress),

            IsOverdueTasks = await _context.Tasks.CountAsync(t => t.Status != TaskState.Closed &&
                t.Status != TaskState.Cancelled &&
                t.Deadline < DateTime.UtcNow),

            OnHoldTasks = await _context.Tasks.CountAsync(t => t.Status == TaskState.OnHold),

            CompletedTasks = await _context.Tasks.CountAsync(t => t.Status == TaskState.Completed),

            CancelledTasks = await _context.Tasks.CountAsync(t => t.Status == TaskState.Cancelled),

            ClosedTasks = await _context.Tasks.CountAsync(t => t.Status == TaskState.Closed),

            TotalComments = await _context.Comments.CountAsync(),

            DeletedComments = await _context.Comments.CountAsync(c => c.IsDeleted),

            TaskHistoryRecords = await _context.TaskHistories.CountAsync(),

            AuditRecords = await _context.AuditLogs.CountAsync()
        };
    }
}