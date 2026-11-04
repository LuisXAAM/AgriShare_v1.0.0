using AgriShare.Domain.Enums;

namespace AgriShare.Application.ServiceExecutions.DTOs;

public sealed record ServiceExecutionDto(
    int Id,
    int ServiceRequestId,
    int OperatorId,
    ServiceExecutionStatus Status,
    DateTime StartedAt,
    DateTime? CompletedAt,
    decimal? ActualSize,
    string? Notes);
