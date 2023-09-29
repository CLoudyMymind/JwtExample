namespace Domain.Models;

public sealed class UserRole
{
    public required int Id { get; set; }
    public required User User { get; set; }
    public required Guid UserId { get; set; }
    public required Role Role { get; set; }
    public required int RoleId { get; set; }
}