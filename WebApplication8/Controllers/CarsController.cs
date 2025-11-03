using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Models;
using WebApplication8.DTOs;
using WebApplication8.Data;

namespace WebApplication8.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CarsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize] // любой авторизованный пользователь
    public async Task<ActionResult<List<Car>>> GetAll() => await _context.Cars.ToListAsync();

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Car>> GetById(int id)
    {
        var car = await _context.Cars.FindAsync(id);
        if (car == null) return NotFound();
        return car;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")] // только Admin
    public async Task<ActionResult<Car>> Create([FromBody] CarDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var car = new Car
        {
            Brand = dto.Brand,
            Model = dto.Model,
            Year = dto.Year,
            Price = dto.Price
        };
        
        _context.Cars.Add(car);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetById), new { id = car.Id }, car);
    }

    [HttpGet("check")]
    [Authorize]
    public ActionResult CheckToken()
    {
        var username = User.Identity?.Name;
        var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        return Ok(new { username, role });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Update(int id, [FromBody] CarDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var car = await _context.Cars.FindAsync(id);
        if (car == null) return NotFound();

        car.Brand = dto.Brand;
        car.Model = dto.Model;
        car.Year = dto.Year;
        car.Price = dto.Price;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        var car = await _context.Cars.FindAsync(id);
        if (car == null) return NotFound();

        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}