using AgriShare.Application.Common.Interfaces;
using AgriShare.Application.Users.DTOs;

namespace AgriShare.Application.Users.Queries;

public sealed record GetUsersQuery;

public sealed class GetUsersQueryHandler(IUnitOfWork unitOfWork)
{
    public async Task<IReadOnlyList<UserDto>> Handle(
        GetUsersQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        var users = await unitOfWork.Users.GetAllAsync(cancellationToken);
        return users.Select(UserDto.FromEntity).ToArray();
    }
}
