using AgriShare.Domain.Enums;

namespace AgriShare.Application.ServiceRequests.DTOs;

public sealed record ServiceRequestDto(
    int Id,
    int FarmerId,
    int ServiceId,
    double Latitude,
    double Longitude,
    decimal EstimatedSize,
    decimal TotalCost,
    RequestStatus Status,
    DateTime ScheduledDate,
    DateTime CreatedAt);
