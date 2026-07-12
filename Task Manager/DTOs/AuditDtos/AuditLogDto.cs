using Task_Manager.Enums;

namespace Task_Manager.DTOs.AuditDtos;

public class AuditLogDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }

    public string Actor { get; set; } = string.Empty;

    public string? TargetUser { get; set; }

    public AuditAction Action { get; set; }

    public string? Description { get; set; }

    public string? Metadata { get; set; }
}
