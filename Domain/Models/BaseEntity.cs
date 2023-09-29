namespace Domain.Models;

public  class BaseEntity
{
    public required Guid Id { get; init; }
    public required DateTimeOffset CreateDateTimeOffset { get; set; }
}