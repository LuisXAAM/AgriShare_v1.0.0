using AgriShare.Application.Common.Interfaces;

namespace AgriShare.Application.Users.Commands;

public sealed record DeactivateUserCommand(int Id);

public sealed class DeactivateUserCommandHandler(IUnitOfWork unitOfWork)
{
    public async Task<bool> Handle(
        DeactivateUserCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var user = await unitOfWork.Users.GetByIdAsync(command.Id, cancellationToken);
        if (user is null)
        {
            return false;
        }

        if (!user.IsActive)
        {
            return true;
        }

        user.IsActive = false;
        await unitOfWork.Users.UpdateAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
