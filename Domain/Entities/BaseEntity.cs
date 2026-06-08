using System.Text.Json.Serialization;

namespace DefenseShield.Domain.Entities;

public abstract class BaseEntity
{
    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
        UpdatedAt = CreatedAt;
    }

    [JsonInclude]
    public Guid Id { get; private set; }

    [JsonInclude]
    public DateTime CreatedAt { get; private set; }

    [JsonInclude]
    public DateTime UpdatedAt { get; private set; }

    public void Touch()
    {
        UpdatedAt = DateTime.Now;
    }
}
