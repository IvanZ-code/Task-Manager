namespace Task_Manager.DTOs.TaskDtos;

public class TaskDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;


    public string Status { get; set; } = string.Empty;


    public string Priority { get; set; } = string.Empty;


    public int CreatorId { get; set; }

    public string Creator { get; set; } = string.Empty;


    public int ExecutorId { get; set; }

    public string Executor { get; set; } = string.Empty;


    public DateTime CreatedAt { get; set; }

    public DateTimeOffset? Deadline { get; set; }

    public bool IsOverdue { get; set; }

    public DateTime? ClosedAt { get; set; }
}
