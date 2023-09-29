namespace Domain.Models;

public sealed class RefreshToken 
{
    public int Id { get; set; }
    public required Guid UserId { get; set; }
    public required string Token { get; set; }
    public required DateTimeOffset RefreshTokenExpiryTime { get; set; }
}