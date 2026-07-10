using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Task_Manager.Data;
using Task_Manager.DTOs.AuthDtos;
using Task_Manager.Models;

namespace Task_Manager.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IConfiguration _configuration;


    public AuthController(DataContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
   
        if (await _context.Users.AnyAsync(u => u.Login == dto.Login))
        {
            return BadRequest(new
            {
                message = "Login is already occupied"
            });
        }

        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
        {
            return BadRequest(new
            {
                message = "Email is already occupied"
            });
        }

        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Login = dto.Login,

            PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(dto.Password),

            Role = UserRole.Employee
        };


        _context.Users.Add(user);

        await _context.SaveChangesAsync();


        return Ok(new
        {
            message = "User registered successfully"
        });
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Login == dto.Login);


        if (user == null)
        {
            return Unauthorized("Invalid login or password");
        }


        bool passwordValid =
            BCrypt.Net.BCrypt.Verify(
                dto.Password,
                user.PasswordHash
            );


        if (!passwordValid)
        {
            return Unauthorized("Invalid login or password");
        }


        var claims = new[]
        {
        new Claim(
            ClaimTypes.NameIdentifier,
            user.Id.ToString()
        ),

        new Claim(
            JwtRegisteredClaimNames.Name,
            user.Login
        ),

        new Claim(
            ClaimTypes.Role,
            user.Role.ToString()
        )
    };


        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!
            )
        );


        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );


        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],

            audience: _configuration["Jwt:Audience"],

            claims: claims,

            expires: DateTime.UtcNow.AddHours(2),

            signingCredentials: credentials
        );


        var tokenString =
            new JwtSecurityTokenHandler()
            .WriteToken(token);


        return Ok(new LoginResponseDto
        {
            Token = tokenString,

            UserId = user.Id,

            Login = user.Login,

            Role = user.Role.ToString()
        });
    }

}
