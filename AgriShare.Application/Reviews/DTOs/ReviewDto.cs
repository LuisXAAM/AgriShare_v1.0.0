namespace AgriShare.Application.Reviews.DTOs;

public sealed class ReviewDto
{
    public int Id { get; init; }
    public int ServiceExecutionId { get; init; }
    public int Rating { get; init; }
    public string? Comment { get; init; }
    public DateTime CreatedAt { get; init; }
}
