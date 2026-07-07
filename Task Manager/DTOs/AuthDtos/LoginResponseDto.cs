namespace Task_Manager.DTOs.AuthDtos;

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;

    public int UserId { get; set; }

    public string Login { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
}
