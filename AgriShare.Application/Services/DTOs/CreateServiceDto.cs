using AgriShare.Domain.Enums;

namespace AgriShare.Application.Services.DTOs;

public sealed record CreateServiceDto(
    int ProviderId,
    int? MachineryId,
    int? OperatorId,
    string Title,
    string LaborType,
    PricingUnit PricingUnit,
    decimal BasePrice,
    decimal TransportFee,
    bool IsActive,
    double Latitude,
    double Longitude);
