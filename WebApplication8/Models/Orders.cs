namespace WebApplication8.Models;

public class Orders
{
    public User user { get; set; }
    public int Id { get; set; }
    public Car car { get; set;  }
    public DateTime OrderDate { get; set; }
}