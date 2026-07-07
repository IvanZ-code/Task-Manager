using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Task_Manager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("public")]
    public IActionResult Public()
    {
        return Ok("Everyone can see this");
    }


    [Authorize]
    [HttpGet("private")]
    public IActionResult Private()
    {
        return Ok("You are authenticated");
    }


    [Authorize(Roles = "Manager")]
    [HttpGet("manager")]
    public IActionResult Manager()
    {
        return Ok("You are a manager");
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public IActionResult Admin()
    {
        return Ok("You are admin");
    }
}
