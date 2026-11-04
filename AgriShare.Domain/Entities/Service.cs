using AgriShare.Domain.Enums;
using NetTopologySuite.Geometries;

namespace AgriShare.Domain.Entities;

public class Service
{
    public int Id { get; set; }
    public int ProviderId { get; set; }
    public int? MachineryId { get; set; }
    public int? OperatorId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string LaborType { get; set; } = string.Empty;
    public PricingUnit PricingUnit { get; set; }
    public decimal BasePrice { get; set; }
    public decimal TransportFee { get; set; }
    public Point Location { get; set; } = null!;
    public Polygon? CoverageArea { get; set; }
    public bool IsActive { get; set; }

    public Provider Provider { get; set; } = null!;
    public Machinery? Machinery { get; set; }
    public Operator? Operator { get; set; }
}
