using AgriShare.Application.Common.Interfaces;
using AgriShare.Application.ServiceExecutions.DTOs;
using AgriShare.Domain.Entities;
using AgriShare.Domain.Enums;

namespace AgriShare.Application.ServiceExecutions.Commands;

public enum StartServiceExecutionFailure
{
    None,
    ServiceRequestNotFound,
    ExecutionAlreadyExists,
    OperatorNotFound
}

public sealed record StartServiceExecutionResult(
    ServiceExecutionDto? Execution,
    StartServiceExecutionFailure Failure = StartServiceExecutionFailure.None);

public sealed record StartServiceExecutionCommand(StartExecutionDto Request);

public sealed class StartServiceExecutionCommandHandler(IUnitOfWork unitOfWork)
{
    public async Task<StartServiceExecutionResult> Handle(
        StartServiceExecutionCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(command.Request);

        var input = command.Request;
        var serviceRequest = await unitOfWork.ServiceRequests.GetByIdAsync(
            input.ServiceRequestId,
            cancellationToken);
        if (serviceRequest is null)
        {
            return new StartServiceExecutionResult(
                null,
                StartServiceExecutionFailure.ServiceRequestNotFound);
        }

        var existingExecution = await unitOfWork.ServiceExecutions.GetByServiceRequestIdAsync(
            input.ServiceRequestId,
            cancellationToken);
        if (existingExecution is not null)
        {
            return new StartServiceExecutionResult(
                null,
                StartServiceExecutionFailure.ExecutionAlreadyExists);
        }

        var operatorEntity = await unitOfWork.Operators.GetByIdAsync(input.OperatorId, cancellationToken);
        if (operatorEntity is null)
        {
            return new StartServiceExecutionResult(null, StartServiceExecutionFailure.OperatorNotFound);
        }

        var startedAt = DateTime.UtcNow;
        var execution = new ServiceExecution
        {
            ServiceRequestId = input.ServiceRequestId,
            OperatorId = input.OperatorId,
            Status = ServiceExecutionStatus.InProgress,
            StartedAt = startedAt
        };

        await unitOfWork.ServiceExecutions.AddAsync(execution, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new StartServiceExecutionResult(new ServiceExecutionDto(
            execution.Id,
            execution.ServiceRequestId,
            input.OperatorId,
            execution.Status,
            startedAt,
            execution.CompletedAt,
            execution.ActualSize,
            execution.FieldNotes));
    }
}
