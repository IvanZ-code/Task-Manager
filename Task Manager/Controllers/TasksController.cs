using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Task_Manager.DTOs.TaskDtos;
using Task_Manager.Interfaces;

namespace Task_Manager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;


    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }


    [Authorize(Roles = "Manager,Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskDto dto)
    {
        var userId = GetUserId();


        var task = await _taskService.CreateTask(
            dto,
            userId
        );


        return Ok(task);
    }


    [Authorize]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyTasks()
    {
        var userId = GetUserId();

        var role = GetUserRole();


        if (role == "Admin")
        {
            return Ok(
                await _taskService.GetAllTasks()
            );
        }


        if (role == "Manager")
        {
            return Ok(
                await _taskService.GetManagerTasks(
                    userId
                )
            );
        }


        return Ok(
            await _taskService.GetEmployeeTasks(
                userId
            )
        );
    }



    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllTasks()
    {
        return Ok(
            await _taskService.GetAllTasks()
        );
    }



    [Authorize]
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        UpdateTaskStatusDto dto)
    {
        var userId = GetUserId();

        var role = GetUserRole();


        var result = await _taskService.UpdateStatus(
            id,
            dto,
            userId,
            role
        );


        if (!result)
        {
            return Forbid();
        }


        return Ok(
            new
            {
                message = "Task status updated"
            }
        );
    }



    private int GetUserId()
    {
        return int.Parse(
            User.FindFirstValue(
                ClaimTypes.NameIdentifier
            )!
        );
    }



    private string GetUserRole()
    {
        return User.FindFirstValue(
            ClaimTypes.Role
        )!;
    }
}
