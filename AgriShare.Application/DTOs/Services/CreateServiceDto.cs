using AgriShare.Domain.Enums;

namespace AgriShare.Application.DTOs.Services;

public class CreateServiceDto
{
    public int ProviderId { get; set; }
    public int MachineryId { get; set; }
    public int OperatorId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string LaborType { get; set; } = string.Empty;
    public PricingUnit PricingUnit { get; set; }
    public decimal BasePrice { get; set; }
    public decimal TransportFee { get; set; }
    public bool IsActive { get; set; }
}
