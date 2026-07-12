using Task_Manager.DTOs.UsersDto;
using Task_Manager.Enums;
using Task_Manager.Models;

namespace Task_Manager.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetUsers();

    Task<bool> ChangeRole(
        int targetUserId,
        int adminId,
        UserRole newRole);

    Task<UserDto?> GetUserByLogin(string login);
}
