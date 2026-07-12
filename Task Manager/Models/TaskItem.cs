using Task_Manager.Enums;

namespace Task_Manager.Models;

public class TaskItem
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public TaskState Status { get; set; }

    public TaskPriority Priority { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTimeOffset? Deadline { get; set; }

    public DateTime? ClosedAt { get; set; }

    public int CreatorId { get; set; }
    public User Creator { get; set; } = null!;

    public int ExecutorId { get; set; }
    public User Executor { get; set; } = null!;

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public ICollection<TaskHistory> History { get; set; }
    = new List<TaskHistory>();
}
