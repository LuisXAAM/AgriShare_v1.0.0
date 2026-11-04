using System.ComponentModel.DataAnnotations;

namespace AgriShare.Application.ServiceExecutions.DTOs;

public sealed record CompleteExecutionDto
{
    [Range(0.01, double.MaxValue)]
    public decimal ActualSize { get; init; }

    public string? Notes { get; init; }
}