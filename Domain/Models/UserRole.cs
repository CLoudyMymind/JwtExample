namespace Domain.Models;

public sealed class UserRole
{
    public UserRole(User user, Guid userId, Role role, int roleId)
    {
        User = user;
        UserId = userId;
        Role = role;
        RoleId = roleId;
    }
    public  int Id { get; set; }
    public  User User { get; set; }
    public  Guid UserId { get; set; }
    public  Role Role { get; set; }
    public  int RoleId { get; set; }
}