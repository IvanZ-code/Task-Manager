namespace Task_Manager.Models;

public class TaskHistory
{
    public int Id { get; set; }

    public int TaskId { get; set; }

    public TaskItem Task { get; set; } = null!;

    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public HistoryAction Action { get; set; }

    public string? Description { get; set; }

    public string? Metadata { get; set; }

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;
}
