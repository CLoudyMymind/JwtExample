using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Context;

public class JwtContext : DbContext
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<ConfirmCode> ConfirmCodes { get; set; }

    public JwtContext(DbContextOptions<JwtContext> options) : base(options)
    {
    }
    
}