using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApplication8.Models;
using WebApplication8.DTOs;

namespace CarShopApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private static List<Car> _cars = new();
    private static int _idCounter = 1;

    [HttpGet]
    [Authorize] // любой авторизованный пользователь
    public ActionResult<List<Car>> GetAll() => _cars;

    [HttpGet("{id}")]
    [Authorize]
    public ActionResult<Car> GetById(int id)
    {
        var car = _cars.FirstOrDefault(c => c.Id == id);
        if (car == null) return NotFound();
        return car;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")] // только Admin
    public ActionResult<Car> Create([FromBody] CarDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var car = new Car
        {
            Id = _idCounter++,
            Brand = dto.Brand,
            Model = dto.Model,
            Year = dto.Year,
            Price = dto.Price
        };
        _cars.Add(car);
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
    public ActionResult Update(int id, [FromBody] CarDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var car = _cars.FirstOrDefault(c => c.Id == id);
        if (car == null) return NotFound();

        car.Brand = dto.Brand;
        car.Model = dto.Model;
        car.Year = dto.Year;
        car.Price = dto.Price;

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public ActionResult Delete(int id)
    {
        var car = _cars.FirstOrDefault(c => c.Id == id);
        if (car == null) return NotFound();

        _cars.Remove(car);
        return NoContent();
    }
}