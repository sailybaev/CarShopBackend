using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Models;
using WebApplication8.DTOs;
using WebApplication8.Services;
using WebApplication8.Data;

namespace WebApplication8.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register([FromBody] UserRegisterDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            return BadRequest("Username already exists.");

        var user = new User
        {
            Username = dto.Username,
            Password = dto.Password,
            Role = dto.Role
        };
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] UserLoginDTO dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username && u.Password == dto.Password);
        if (user == null) return Unauthorized("Invalid credentials.");

        var token = TokenService.GenerateToken(user);
        return Ok(token);
    }
}