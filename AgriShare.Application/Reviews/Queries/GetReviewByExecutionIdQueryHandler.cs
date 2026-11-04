using AgriShare.Application.Common.Interfaces;
using AgriShare.Application.Reviews.DTOs;
using MediatR;

namespace AgriShare.Application.Reviews.Queries;

public sealed record GetReviewByExecutionIdQuery(int ServiceExecutionId) : IRequest<ReviewDto?>;

public sealed class GetReviewByExecutionIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetReviewByExecutionIdQuery, ReviewDto?>
{
    public async Task<ReviewDto?> Handle(
        GetReviewByExecutionIdQuery request,
        CancellationToken cancellationToken)
    {
        var review = await unitOfWork.Reviews.GetByExecutionIdAsync(
            request.ServiceExecutionId,
            cancellationToken);

        return review is null
            ? null
            : new ReviewDto
            {
                Id = review.Id,
                ServiceExecutionId = review.ServiceExecutionId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
    }
}
