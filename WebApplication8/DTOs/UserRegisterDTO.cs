namespace WebApplication8.DTOs;
using System.ComponentModel.DataAnnotations;
public class UserRegisterDTO
{
    [Required(ErrorMessage = "Username обязательно")]
    [MinLength(3, ErrorMessage = "Username должен быть минимум 3 символа")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password обязательно")]
    [MinLength(6, ErrorMessage = "Password должен быть минимум 6 символов")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [RegularExpression("Admin|Client", ErrorMessage = "Role должен быть Admin или Client")]
    public string Role { get; set; } = "Client";
}
