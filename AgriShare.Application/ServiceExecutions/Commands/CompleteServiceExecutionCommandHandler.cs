using AgriShare.Application.Common.Interfaces;
using AgriShare.Application.ServiceExecutions.DTOs;
using AgriShare.Domain.Enums;

namespace AgriShare.Application.ServiceExecutions.Commands;

public sealed record CompleteServiceExecutionCommand(int Id, CompleteExecutionDto Completion);

public sealed class CompleteServiceExecutionCommandHandler(IUnitOfWork unitOfWork)
{
    public async Task<bool> Handle(
        CompleteServiceExecutionCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(command.Completion);

        if (command.Completion.ActualSize <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(command), "Actual size must be greater than zero.");
        }

        var execution = await unitOfWork.ServiceExecutions.GetByIdAsync(command.Id, cancellationToken);
        if (execution is null || execution.Status != ServiceExecutionStatus.InProgress)
        {
            return false;
        }

        execution.Status = ServiceExecutionStatus.Completed;
        execution.CompletedAt = DateTime.UtcNow;
        execution.ActualSize = command.Completion.ActualSize;
        execution.FieldNotes = command.Completion.Notes?.Trim();

        await unitOfWork.ServiceExecutions.UpdateAsync(execution, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
