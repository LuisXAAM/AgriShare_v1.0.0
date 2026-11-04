namespace AgriShare.Domain.Entities;

public class Review
{
    public int Id { get; set; }
    public int ServiceRequestId { get; set; }
    public int ReviewerId { get; set; }
    public int RevieweeId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public ServiceRequest ServiceRequest { get; set; } = null!;
    public User Reviewer { get; set; } = null!;
    public User Reviewee { get; set; } = null!;
}
