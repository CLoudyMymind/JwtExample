namespace Domain.Models;

public  class BaseEntity
{
    public  Guid Id { get; init; }
    public  DateTimeOffset CreateDateTimeOffset { get; set; }
}