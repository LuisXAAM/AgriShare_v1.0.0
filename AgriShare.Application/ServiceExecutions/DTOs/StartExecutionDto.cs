using System.ComponentModel.DataAnnotations;

namespace AgriShare.Application.ServiceExecutions.DTOs;

public sealed record StartExecutionDto
{
    [Range(1, int.MaxValue)]
    public int ServiceRequestId { get; init; }

    [Range(1, int.MaxValue)]
    public int OperatorId { get; init; }
}