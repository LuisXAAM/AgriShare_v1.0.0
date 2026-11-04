namespace AgriShare.Domain.Entities;

public class Provider
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public bool VerifiedStatus { get; set; }

    public User User { get; set; } = null!;
    public ICollection<Machinery> Machinery { get; set; } = new List<Machinery>();
    public ICollection<Operator> Operators { get; set; } = new List<Operator>();
    public ICollection<Service> Services { get; set; } = new List<Service>();
}
