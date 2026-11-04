using AgriShare.Application.Common.Interfaces;
using AgriShare.Application.Users.DTOs;
using AgriShare.Domain.Entities;
using AgriShare.Domain.Enums;

namespace AgriShare.Application.Users.Commands;

public sealed record CreateUserCommand(CreateUserDto User);

public sealed record CreateUserResult(UserDto? User, bool EmailAlreadyExists);

public sealed class CreateUserCommandHandler(IUnitOfWork unitOfWork)
{
    public async Task<CreateUserResult> Handle(
        CreateUserCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(command.User);

        var input = command.User;
        var email = input.Email.Trim();
        if (await unitOfWork.Users.EmailExistsAsync(email, cancellationToken))
        {
            return new CreateUserResult(null, EmailAlreadyExists: true);
        }

        var role = input.Role switch
        {
            1 => UserRole.Provider,
            2 => UserRole.Farmer,
            _ => throw new ArgumentOutOfRangeException(
                nameof(command), "Role must be 1 (Provider) or 2 (Farmer).")
        };

        var user = new User
        {
            FullName = input.FullName.Trim(),
            Email = email,
            PasswordHash = input.PasswordHash,
            Role = role,
            PhoneNumber = input.PhoneNumber.Trim(),
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await unitOfWork.Users.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateUserResult(UserDto.FromEntity(user), EmailAlreadyExists: false);
    }
}
