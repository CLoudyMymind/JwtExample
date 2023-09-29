namespace Domain.Models;

public class ConfirmCode
{
    public  int Id { get; set; }
    public required User User { get; set; }
    public required Guid UserId { get; set; }
    public required string Codes { get; set; }
    public required DateTimeOffset ExpiredDateTime { get; set; }
}