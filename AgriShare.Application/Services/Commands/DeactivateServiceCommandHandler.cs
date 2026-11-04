using AgriShare.Application.Common.Interfaces;

namespace AgriShare.Application.Services.Commands;

public sealed record DeactivateServiceCommand(int Id);

public sealed class DeactivateServiceCommandHandler(IUnitOfWork unitOfWork)
{
    public async Task<bool> Handle(
        DeactivateServiceCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var service = await unitOfWork.Services.GetByIdAsync(command.Id, cancellationToken);
        if (service is null)
        {
            return false;
        }

        if (!service.IsActive)
        {
            return true;
        }

        service.IsActive = false;
        await unitOfWork.Services.UpdateAsync(service, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
