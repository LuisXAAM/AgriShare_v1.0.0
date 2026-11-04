using AgriShare.Application.Common.Interfaces;
using AgriShare.Application.Users.DTOs;

namespace AgriShare.Application.Users.Commands;

public sealed record UpdateUserCommand(int Id, UpdateUserDto User);

public sealed class UpdateUserCommandHandler(IUnitOfWork unitOfWork)
{
    public async Task<UserDto?> Handle(
        UpdateUserCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(command.User);

        var user = await unitOfWork.Users.GetByIdAsync(command.Id, cancellationToken);
        if (user is null)
        {
            return null;
        }

        user.FullName = command.User.FullName.Trim();
        user.PhoneNumber = command.User.PhoneNumber.Trim();

        await unitOfWork.Users.UpdateAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return UserDto.FromEntity(user);
    }
}
