using Microsoft.EntityFrameworkCore;
using System;
using Task_Manager.Data;
using Task_Manager.DTOs.TaskDtos;
using Task_Manager.Enums;
using Task_Manager.Interfaces;
using Task_Manager.Models;

namespace Task_Manager.Services;

public class TaskService : ITaskService
{
    private readonly DataContext _context;

    private readonly IHistoryService _historyService;


    public TaskService(DataContext context, IHistoryService historyService)
    {
        _context = context;
        _historyService = historyService;
    }



    public async Task<TaskDto> CreateTask(
        CreateTaskDto dto,
        int managerId)
    {
        var manager = await _context.Users
       .FirstOrDefaultAsync(u => u.Id == managerId);

        if (manager == null)
            throw new KeyNotFoundException("managerId is not found");

        var executor = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == dto.ExecutorId);

        if (executor == null)
            throw new KeyNotFoundException("ExecutorId is not found");


        switch (manager.Role)
        {
            case UserRole.Manager:
                if (executor.Role != UserRole.Employee)
                    throw new UnauthorizedAccessException("Manager can assign tasks only to employees");
                break;

            case UserRole.Admin:
                if (executor.Role == UserRole.Admin)
                    throw new UnauthorizedAccessException("Admin can't assign tasks to another admin");
                break;

            default:
                throw new UnauthorizedAccessException("You have no rights");
        }

        

        var task = new TaskItem
        {
            Title = dto.Title,

            Description = dto.Description,

            Priority = dto.Priority,

            Status = TaskState.New,

            CreatorId = managerId,

            ExecutorId = dto.ExecutorId,

            Deadline = dto.Deadline?.UtcDateTime
        };


        _context.Tasks.Add(task);

        await _context.SaveChangesAsync();

        await _historyService.AddRecord(
            task.Id,
            managerId,
            HistoryAction.TaskCreated,
            $"Task '{task.Title}' created."
        );

        return await MapTask(task);
    }



    public async Task<IEnumerable<TaskDto>> GetEmployeeTasks(
    int employeeId)
    {
        var tasks = await _context.Tasks
            .Include(t => t.Creator)
            .Include(t => t.Executor)
            .Where(t => t.ExecutorId == employeeId)
            .ToListAsync();


        return tasks.Select(MapTaskSync);
    }

    public async Task<ManagerTasksDto> GetManagerTasks(
    int managerId)
    {
        var created = await _context.Tasks
        .Include(t => t.Creator)
        .Include(t => t.Executor)
        .Where(t => t.CreatorId == managerId)
        .ToListAsync();

        var assigned = await _context.Tasks
            .Include(t => t.Creator)
            .Include(t => t.Executor)
            .Where(t => t.ExecutorId == managerId)
            .ToListAsync();

        return new ManagerTasksDto
        {
            CreatedTasks = created.Select(MapTaskSync),
            AssignedTasks = assigned.Select(MapTaskSync)
        };
    }

    public async Task<IEnumerable<TaskDto>> GetAllTasks()
    {
        var tasks = await _context.Tasks
            .Include(t => t.Creator)
            .Include(t => t.Executor)
            .ToListAsync();


        return tasks.Select(MapTaskSync);
    }

    public async Task<bool> UpdateStatus(
    int taskId,
    UpdateTaskStatusDto dto,
    int userId,
    string role)
    {

        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == taskId);


        if (task == null)
            return false;


        if (task.Status == TaskState.Closed ||
        task.Status == TaskState.Cancelled || task.Status == dto.Status)
        {
            return false;
        }

        bool allowed = false;


        if (role == "Admin")
        {
            allowed = true;
        }


        if (task.CreatorId == userId)
        {
            allowed = true;
        }


        if (task.ExecutorId == userId)
        {
            allowed = true;
        }


        if (!allowed)
            return false;


        if (role == "Employee" &&
        (dto.Status == TaskState.Closed ||
         dto.Status == TaskState.Cancelled))
        {
            return false;
        }

        var oldStatus = task.Status;

        task.Status = dto.Status;


        if (dto.Status == TaskState.Closed || dto.Status == TaskState.Cancelled)
        {
            task.ClosedAt = DateTime.UtcNow;
        }


        await _context.SaveChangesAsync();

        await _historyService.AddRecord(
            task.Id,
            userId,
            HistoryAction.StatusChanged,
            $"Status changed: {oldStatus} → {task.Status}"

        );

        if (task.Status == TaskState.Closed || task.Status == TaskState.Cancelled)
        {
            await _historyService.AddRecord(
                task.Id,
                userId,
                HistoryAction.TaskClosed,
                "Task closed."
            );
        }

        return true;
    }



    private async Task<TaskDto> MapTask(
        TaskItem task)
    {
        await _context.Entry(task)
            .Reference(t => t.Creator)
            .LoadAsync();


        await _context.Entry(task)
            .Reference(t => t.Executor)
            .LoadAsync();


        return MapTaskSync(task);
    }



    private TaskDto MapTaskSync(
        TaskItem task)
    {

        return new TaskDto
        {
            Id = task.Id,

            Title = task.Title,

            Description = task.Description,

            Status = task.Status.ToString(),

            Priority = task.Priority.ToString(),

            CreatorId = task.Creator.Id,

            Creator = task.Creator.Login,

            ExecutorId = task.Executor.Id,

            Executor = task.Executor.Login,

            CreatedAt = task.CreatedAt,

            Deadline = task.Deadline,

            IsOverdue =
                task.Status != TaskState.Closed &&
                task.Status != TaskState.Cancelled &&
                task.Deadline < DateTime.UtcNow,

            ClosedAt = task.ClosedAt

            
        };
    }
}
