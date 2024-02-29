namespace Evermore.Models;
public class User
{
    public UserId Id { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public readonly record struct UserId(Guid Value);
