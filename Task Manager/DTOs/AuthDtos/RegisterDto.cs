using System.ComponentModel.DataAnnotations;

namespace Task_Manager.DTOs.AuthDtos;

public class RegisterDto
{
    [Required(ErrorMessage = "FirstName is required")]
    [StringLength(20, MinimumLength = 2,
        ErrorMessage = "FirstName must contain 2 to 20 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "LastName is required")]
    [StringLength(20, MinimumLength = 2,
        ErrorMessage = "LastName must contain 2 to 20 characters")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Incorrect email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Login is required")]
    [RegularExpression("^[a-z]+$",
        ErrorMessage = "Login must contain only lowercase Latin letters")]
    [StringLength(22, MinimumLength = 4,
        ErrorMessage = "Login must contain 4 to 22 characters")]
    public string Login { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 8,
        ErrorMessage = "Password must contain at least 8 characters")]
    public string Password { get; set; } = string.Empty;
}
