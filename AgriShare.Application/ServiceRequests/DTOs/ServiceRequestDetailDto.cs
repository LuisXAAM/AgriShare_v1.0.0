using AgriShare.Domain.Enums;

namespace AgriShare.Application.ServiceRequests.DTOs;

public sealed record ServiceRequestDetailDto(
    int Id,
    int FarmerId,
    string FarmerName,
    int ServiceId,
    string ServiceTitle,
    string ProviderName,
    double Latitude,
    double Longitude,
    decimal EstimatedSize,
    decimal TotalCost,
    RequestStatus Status,
    DateTime ScheduledDate,
    DateTime CreatedAt);
