namespace WebApplication8.DTOs;
using System.ComponentModel.DataAnnotations;

public class CarDTO
{
    [Required(ErrorMessage = "Brand обязательно")]
    public string Brand { get; set; } = string.Empty;

    [Required(ErrorMessage = "Model обязательно")]
    public string Model { get; set; } = string.Empty;

    [Range(1900, 2100, ErrorMessage = "Year должен быть от 1900 до 2100")]
    public int Year { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Price должен быть больше 0")]
    public decimal Price { get; set; }
}