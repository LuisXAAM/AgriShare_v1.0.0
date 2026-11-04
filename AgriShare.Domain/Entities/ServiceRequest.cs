using AgriShare.Domain.Enums;
using NetTopologySuite.Geometries;

namespace AgriShare.Domain.Entities;

public class ServiceRequest
{
    public int Id { get; set; }
    public int FarmerId { get; set; }
    public int ServiceId { get; set; }
    public Point FarmLocation { get; set; } = null!;
    public decimal EstimatedSize { get; set; }
    public decimal TotalCost { get; set; }
    public RequestStatus Status { get; set; }
    public DateTime ScheduledDate { get; set; }
    public DateTime CreatedAt { get; set; }

    public User Farmer { get; set; } = null!;
    public Service Service { get; set; } = null!;
    public ServiceExecution? ServiceExecution { get; set; }
}
