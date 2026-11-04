namespace AgriShare.Domain.Entities;

public class Machinery
{
    public int Id { get; set; }
    public int ProviderId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public Provider Provider { get; set; } = null!;
}
