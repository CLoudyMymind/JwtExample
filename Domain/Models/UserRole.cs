namespace Domain.Models;

public sealed class UserRole
{
    public UserRole(User user, Guid userId, int roleId, Role role)
    {
        User = user;
        UserId = userId;
        RoleId = roleId;
        Role = role;
    }

    public UserRole()
    {
        
    }
    public  int Id { get; set; }
    public  User User { get; set; }
    public  Guid UserId { get; set; }
    public  Role Role { get; set; }
    public  int RoleId { get; set; }
}