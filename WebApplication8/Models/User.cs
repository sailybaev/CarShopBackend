namespace WebApplication8.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // на проде — хранить хэш!
    public string Role { get; set; } = "Client"; // Admin / Client
}
