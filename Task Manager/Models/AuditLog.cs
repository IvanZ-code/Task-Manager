using Task_Manager.Enums;

namespace Task_Manager.Models;

public class AuditLog
{
    public int Id { get; set; }

    public int ActorId { get; set; }

    public User Actor { get; set; } = null!;

    public int? TargetUserId { get; set; }

    public User? TargetUser { get; set; }

    public AuditAction Action { get; set; }

    public string? Description { get; set; }

    public string? Metadata { get; set; }

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;
}
