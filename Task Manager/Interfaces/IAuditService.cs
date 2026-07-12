using Task_Manager.DTOs.AuditDtos;
using Task_Manager.Enums;

namespace Task_Manager.Interfaces;

public interface IAuditService
{
    Task AddRecord(
        int actorId,
        AuditAction action,
        int? targetUserId = null,
        string? description = null,
        string? metadata = null);

    Task<IEnumerable<AuditLogDto>> GetAuditLog();
}
