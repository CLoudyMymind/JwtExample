namespace Domain.Models;

public sealed class Token
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}