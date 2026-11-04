using AgriShare.Domain.Entities;
using AgriShare.Domain.Enums;

namespace AgriShare.Application.Services.DTOs;

public sealed record ServiceDto(
    int Id,
    int ProviderId,
    int? MachineryId,
    int? OperatorId,
    string Title,
    string LaborType,
    PricingUnit PricingUnit,
    decimal BasePrice,
    decimal TransportFee,
    bool IsActive,
    double? Latitude,
    double? Longitude,
    ServiceProviderDto? ProviderDetails = null,
    ServiceMachineryDto? MachineryDetails = null,
    ServiceOperatorDto? OperatorDetails = null)
{
    public static ServiceDto FromEntity(Service service)
    {
        ArgumentNullException.ThrowIfNull(service);

        return new ServiceDto(
            service.Id,
            service.ProviderId,
            service.MachineryId,
            service.OperatorId,
            service.Title,
            service.LaborType,
            service.PricingUnit,
            service.BasePrice,
            service.TransportFee,
            service.IsActive,
            service.Location?.Y,
            service.Location?.X,
            service.Provider is null
                ? null
                : new ServiceProviderDto(
                    service.Provider.Id,
                    service.Provider.BusinessName,
                    service.Provider.VerifiedStatus),
            service.Machinery is null
                ? null
                : new ServiceMachineryDto(
                    service.Machinery.Id,
                    service.Machinery.Name,
                    service.Machinery.Type,
                    service.Machinery.Description),
            service.Operator is null
                ? null
                : new ServiceOperatorDto(
                    service.Operator.Id,
                    service.Operator.FullName,
                    service.Operator.LicenseNumber,
                    service.Operator.Status));
    }
}

public sealed record ServiceProviderDto(int Id, string BusinessName, bool VerifiedStatus);

public sealed record ServiceMachineryDto(int Id, string Name, string Type, string Description);

public sealed record ServiceOperatorDto(
    int Id,
    string FullName,
    string LicenseNumber,
    OperatorStatus Status);
