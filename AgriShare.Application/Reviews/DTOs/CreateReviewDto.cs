using System.ComponentModel.DataAnnotations;

namespace AgriShare.Application.Reviews.DTOs;

public sealed class CreateReviewDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ServiceExecutionId { get; init; }

    [Required]
    [Range(1, 5)]
    public int Rating { get; init; }

    [StringLength(500)]
    public string? Comment { get; init; }
}
