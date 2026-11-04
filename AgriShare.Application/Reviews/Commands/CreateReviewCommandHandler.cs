using AgriShare.Application.Common.Interfaces;
using AgriShare.Application.Reviews.DTOs;
using AgriShare.Domain.Entities;
using AgriShare.Domain.Enums;
using MediatR;

namespace AgriShare.Application.Reviews.Commands;

public enum CreateReviewFailure
{
    None,
    ServiceExecutionNotFound,
    ServiceExecutionNotCompleted,
    ReviewAlreadyExists
}

public sealed record CreateReviewResult(
    ReviewDto? Review,
    CreateReviewFailure Failure = CreateReviewFailure.None);

public sealed record CreateReviewCommand(CreateReviewDto Review) : IRequest<CreateReviewResult>;

public sealed class CreateReviewCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateReviewCommand, CreateReviewResult>
{
    public async Task<CreateReviewResult> Handle(
        CreateReviewCommand request,
        CancellationToken cancellationToken)
    {
        var execution = await unitOfWork.ServiceExecutions.GetByIdAsync(
            request.Review.ServiceExecutionId,
            cancellationToken);

        if (execution is null)
        {
            return new CreateReviewResult(null, CreateReviewFailure.ServiceExecutionNotFound);
        }

        if (execution.Status != ServiceExecutionStatus.Completed)
        {
            return new CreateReviewResult(null, CreateReviewFailure.ServiceExecutionNotCompleted);
        }

        var existingReview = await unitOfWork.Reviews.GetByExecutionIdAsync(
            request.Review.ServiceExecutionId,
            cancellationToken);
        if (existingReview is not null)
        {
            return new CreateReviewResult(null, CreateReviewFailure.ReviewAlreadyExists);
        }

        var review = new Review(
            request.Review.ServiceExecutionId,
            request.Review.Rating,
            request.Review.Comment);

        await unitOfWork.Reviews.AddAsync(review, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateReviewResult(new ReviewDto
        {
            Id = review.Id,
            ServiceExecutionId = review.ServiceExecutionId,
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt
        });
    }
}
