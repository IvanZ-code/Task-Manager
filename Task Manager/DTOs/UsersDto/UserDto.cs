using Task_Manager.Enums;

namespace Task_Manager.DTOs.UsersDto;

public class UserDto
{
    public int Id { get; set; }

    public string Login { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public UserRole Role { get; set; }
}
