using AgriShare.Application.ServiceExecutions.Commands;
using AgriShare.Application.ServiceExecutions.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AgriShare.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ServiceExecutionsController(
    StartServiceExecutionCommandHandler startExecutionHandler,
    CompleteServiceExecutionCommandHandler completeExecutionHandler) : ControllerBase
{
    [HttpPost("start")]
    public async Task<IActionResult> StartExecution(
        [FromBody] StartExecutionDto request,
        CancellationToken cancellationToken)
    {
        var result = await startExecutionHandler.Handle(
            new StartServiceExecutionCommand(request),
            cancellationToken);

        return result.Failure switch
        {
            StartServiceExecutionFailure.ServiceRequestNotFound =>
                NotFound("The service request was not found."),
            StartServiceExecutionFailure.ExecutionAlreadyExists =>
                Conflict("This service request already has an execution."),
            StartServiceExecutionFailure.OperatorNotFound =>
                BadRequest("The specified operator was not found."),
            StartServiceExecutionFailure.None when result.Execution is not null =>
                CreatedAtAction(
                    nameof(CompleteExecution),
                    new { id = result.Execution.Id },
                    result.Execution),
            _ => throw new InvalidOperationException("Service execution could not be started.")
        };
    }

    [HttpPut("{id:int}/complete")]
    public async Task<IActionResult> CompleteExecution(
        int id,
        [FromBody] CompleteExecutionDto request,
        CancellationToken cancellationToken)
    {
        var completed = await completeExecutionHandler.Handle(
            new CompleteServiceExecutionCommand(id, request),
            cancellationToken);

        return completed
            ? NoContent()
            : NotFound("The execution was not found or is not in progress.");
    }
}
