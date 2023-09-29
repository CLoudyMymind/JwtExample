namespace Domain.Models;

public sealed class User : BaseEntity
{
    public required string Login { get; set; }
    public required string Password { get; set; }
    public required string Surname { get; set; }
    public required string Email { get; set; }
    public required bool UserConfirmed { get; set; } 
}