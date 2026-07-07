using Microsoft.EntityFrameworkCore;
using System;
using Task_Manager.Data;
using Task_Manager.DTOs.TaskDtos;
using Task_Manager.Interfaces;
using Task_Manager.Models;

namespace Task_Manager.Services;

public class TaskService : ITaskService
{
    private readonly DataContext _context;


    public TaskService(DataContext context)
    {
        _context = context;
    }



    public async Task<TaskDto> CreateTask(
        CreateTaskDto dto,
        int managerId)
    {

        var task = new TaskItem
        {
            Title = dto.Title,

            Description = dto.Description,

            Priority = dto.Priority,

            Status = TaskState.New,

            CreatorId = managerId,

            ExecutorId = dto.ExecutorId
        };


        _context.Tasks.Add(task);

        await _context.SaveChangesAsync();


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

    public async Task<IEnumerable<TaskDto>> GetManagerTasks(
    int managerId)
    {
        var tasks = await _context.Tasks
            .Include(t => t.Creator)
            .Include(t => t.Executor)
            .Where(t => t.CreatorId == managerId)
            .ToListAsync();


        return tasks.Select(MapTaskSync);
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



        task.Status = dto.Status;


        if (dto.Status == TaskState.Closed)
        {
            task.ClosedAt = DateTime.UtcNow;
        }


        await _context.SaveChangesAsync();


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

            Creator = task.Creator.Login,

            Executor = task.Executor.Login
        };
    }
}
