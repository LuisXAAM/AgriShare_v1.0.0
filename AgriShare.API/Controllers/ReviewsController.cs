using AgriShare.Application.Reviews.Commands;
using AgriShare.Application.Reviews.DTOs;
using AgriShare.Application.Reviews.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AgriShare.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ReviewsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateReviewDto request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateReviewCommand(request), cancellationToken);

        return result.Failure switch
        {
            CreateReviewFailure.ServiceExecutionNotFound =>
                NotFound("The service execution was not found."),
            CreateReviewFailure.ServiceExecutionNotCompleted =>
                BadRequest("A review can only be created for a completed service execution."),
            CreateReviewFailure.ReviewAlreadyExists =>
                Conflict("A review already exists for this service execution."),
            CreateReviewFailure.None when result.Review is not null =>
                CreatedAtAction(
                    nameof(GetByExecutionId),
                    new { executionId = result.Review.ServiceExecutionId },
                    result.Review),
            _ => throw new InvalidOperationException("The review could not be created.")
        };
    }

    [HttpGet("execution/{executionId:int}")]
    public async Task<ActionResult<ReviewDto>> GetByExecutionId(
        int executionId,
        CancellationToken cancellationToken)
    {
        var review = await mediator.Send(
            new GetReviewByExecutionIdQuery(executionId),
            cancellationToken);

        return review is null ? NotFound() : Ok(review);
    }
}
