using AgriShare.Application.Common.Interfaces;
using AgriShare.Application.Users.DTOs;

namespace AgriShare.Application.Users.Queries;

public sealed record GetUserByIdQuery(int Id);

public sealed class GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
{
    public async Task<UserDto?> Handle(
        GetUserByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        var user = await unitOfWork.Users.GetByIdAsync(query.Id, cancellationToken);
        return user is null ? null : UserDto.FromEntity(user);
    }
}
