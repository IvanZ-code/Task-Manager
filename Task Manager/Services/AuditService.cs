using Microsoft.EntityFrameworkCore;
using Task_Manager.Data;
using Task_Manager.DTOs.AuditDtos;
using Task_Manager.Enums;
using Task_Manager.Interfaces;
using Task_Manager.Models;

namespace Task_Manager.Services;

public class AuditService : IAuditService
{
    private readonly DataContext _context;

    public AuditService(DataContext context)
    {
        _context = context;
    }

    public async Task AddRecord(
        int actorId,
        AuditAction action,
        int? targetUserId = null,
        string? description = null,
        string? metadata = null)
    {
        var audit = new AuditLog
        {
            ActorId = actorId,
            TargetUserId = targetUserId,
            Action = action,
            Description = description,
            Metadata = metadata,
            CreatedAt = DateTime.UtcNow
        };

        _context.AuditLogs.Add(audit);

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<AuditLogDto>> GetAuditLog()
    {
        return await _context.AuditLogs
            .Include(a => a.Actor)
            .Include(a => a.TargetUser)
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new AuditLogDto
            {
                Id = a.Id,
                CreatedAt = a.CreatedAt,
                Actor = a.Actor.Login,
                TargetUser = a.TargetUser != null
                    ? a.TargetUser.Login
                    : null,
                Action = a.Action,
                Description = a.Description,
                Metadata = a.Metadata
            })
            .ToListAsync();
    }
}
