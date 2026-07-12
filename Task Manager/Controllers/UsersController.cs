using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Task_Manager.DTOs.UsersDto;
using Task_Manager.Interfaces;

namespace Task_Manager.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize(Roles = "Manager, Admin")]
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await _userService.GetUsers());
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/role")]
    public async Task<IActionResult> ChangeRole(
        int id,
        UpdateRoleDto dto)
    {
        var adminId = int.Parse(
            User.FindFirstValue(
                ClaimTypes.NameIdentifier)!);

        await _userService.ChangeRole(
            id,
            adminId,
            dto.Role);

        return Ok(new
        {
            message = "Role updated."
        });
    }

    [Authorize(Roles = "Manager, Admin")]
    [HttpGet("login/{login}")]
    public async Task<IActionResult> GetUserByLogin(string login)
    {
        var user = await _userService.GetUserByLogin(login);

        if (user == null)
            return NotFound();

        return Ok(user);
    }
}
