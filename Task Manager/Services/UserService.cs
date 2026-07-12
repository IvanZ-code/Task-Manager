using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Text.Json;
using Task_Manager.Data;
using Task_Manager.DTOs.CommentDtos;
using Task_Manager.DTOs.UsersDto;
using Task_Manager.Enums;
using Task_Manager.Interfaces;
using Task_Manager.Models;

namespace Task_Manager.Services;

public class UserService : IUserService
{
    private readonly DataContext _context;

    private readonly IAuditService _auditService;

    public UserService(DataContext context, IAuditService auditService)
    {
        _context = context; 
        _auditService = auditService;
    }

    public async Task<IEnumerable<UserDto>> GetUsers()
    {
        return await _context.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Login = u.Login,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Role = u.Role
            })
            .ToListAsync();
    }
    public async Task<bool> ChangeRole(
    int targetUserId,
    int adminId,
    UserRole newRole)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == targetUserId);
        var admin = await _context.Users
            .FirstOrDefaultAsync (u => u.Id == adminId);

        if (user == null)
            throw new KeyNotFoundException("User not found.");

        if (admin == null)
            throw new KeyNotFoundException("Admin not found.");

        if (user.Role == UserRole.Admin)
            throw new UnauthorizedAccessException(
                "Cannot change another administrator's role.");

        if (newRole == UserRole.Admin)
            throw new UnauthorizedAccessException(
                "Cannot change another role to admin.");

        if (user.Role == newRole)
            throw new InvalidOperationException("Role has not been changed");

        var oldRole = user.Role;

        user.Role = newRole;

        await _context.SaveChangesAsync();

        await _auditService.AddRecord(
            actorId: adminId,
            action: AuditAction.UserRoleChanged,
            targetUserId: user.Id,
            description: $"Changed role of '{user.Login}' from {oldRole} to {newRole} by admin '{admin.Login}'.",
            metadata: JsonSerializer.Serialize(new
            {
                OldRole = oldRole,
                NewRole = newRole
            }));

        return true;
    }

    public async Task<UserDto?> GetUserByLogin(string login)
    {
        return await _context.Users
        .Where(u => EF.Functions.ILike(u.Login, login))
        .Select(u => new UserDto
        {
            Id = u.Id,
            Login = u.Login,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
            Role = u.Role
        })
        .FirstOrDefaultAsync();
    }
}
