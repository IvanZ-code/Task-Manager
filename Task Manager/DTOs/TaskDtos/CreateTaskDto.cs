using Task_Manager.Models;

namespace Task_Manager.DTOs.TaskDtos;

public class CreateTaskDto
{
    public string Title { get; set; } = string.Empty;


    public string Description { get; set; } = string.Empty;


    public TaskPriority Priority { get; set; }

    public DateTime? Deadline { get; set; }


    public int ExecutorId { get; set; }
}
