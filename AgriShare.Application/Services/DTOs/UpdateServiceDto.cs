using AgriShare.Domain.Enums;

namespace AgriShare.Application.Services.DTOs;

public sealed record UpdateServiceDto(
    string Title,
    string LaborType,
    PricingUnit PricingUnit,
    decimal BasePrice,
    decimal TransportFee,
    bool IsActive,
    double? Latitude = null,
    double? Longitude = null);
