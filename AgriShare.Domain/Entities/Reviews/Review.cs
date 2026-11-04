namespace AgriShare.Domain.Entities;

public sealed class Review
{
    private Review()
    {
    }

    public Review(int serviceExecutionId, int rating, string? comment)
    {
        if (serviceExecutionId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(serviceExecutionId));
        }

        if (rating is < 1 or > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");
        }

        if (comment?.Length > 500)
        {
            throw new ArgumentException("Comment cannot exceed 500 characters.", nameof(comment));
        }

        ServiceExecutionId = serviceExecutionId;
        Rating = rating;
        Comment = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim();
        CreatedAt = DateTime.UtcNow;
    }

    public int Id { get; private set; }
    public int ServiceExecutionId { get; private set; }
    public int Rating { get; private set; }
    public string? Comment { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public ServiceExecution ServiceExecution { get; private set; } = null!;
}
