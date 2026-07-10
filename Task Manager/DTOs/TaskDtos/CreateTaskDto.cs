using System.ComponentModel.DataAnnotations;
using Task_Manager.Models;

namespace Task_Manager.DTOs.TaskDtos;

public class CreateTaskDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, MinimumLength = 4,
        ErrorMessage = "Title must contain 4 to 50 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    [StringLength(100000, MinimumLength = 4,
        ErrorMessage = "Description must contain 4 to 100000 characters")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Priority is required")]
    [EnumDataType(typeof(TaskPriority), ErrorMessage = "Incorrect priority value")]
    public TaskPriority Priority { get; set; }

    public DateTimeOffset? Deadline { get; set; }

    [Required(ErrorMessage = "ExecutorId is required")]
    public int ExecutorId { get; set; }
}
