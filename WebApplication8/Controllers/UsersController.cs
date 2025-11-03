using Microsoft.AspNetCore.Mvc;
using WebApplication8.Models;
using WebApplication8.DTOs;
using WebApplication8.Services;

namespace WebApplication8.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private static List<User> _users = new();
    private static int _idCounter = 1;

    [HttpPost("register")]
    public ActionResult<User> Register([FromBody] UserRegisterDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (_users.Any(u => u.Username == dto.Username))
            return BadRequest("Username already exists.");

        var user = new User
        {
            Id = _idCounter++,
            Username = dto.Username,
            Password = dto.Password,
            Role = dto.Role
        };
        _users.Add(user);
        
        
        return Ok(user);
    }

    [HttpPost("login")]
    public ActionResult<string> Login([FromBody] UserLoginDTO dto)
    {
        var user = _users.FirstOrDefault(u => u.Username == dto.Username && u.Password == dto.Password);
        if (user == null) return Unauthorized("Invalid credentials.");

        var token = TokenService.GenerateToken(user);
        return Ok(token);
    }

    [HttpGet("all")]
    public ActionResult<List<User>> GetAllUsers()
    {
        return Ok(_users);
    }
}