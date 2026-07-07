namespace Task_Manager.Models;

public class Comment
{
    public int Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int TaskItemId { get; set; }
    public TaskItem TaskItem { get; set; } = null!;

    public int AuthorId { get; set; }
    public User Author { get; set; } = null!;
}
