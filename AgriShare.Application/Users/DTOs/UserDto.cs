using AgriShare.Domain.Entities;
using AgriShare.Domain.Enums;

namespace AgriShare.Application.Users.DTOs;

public sealed record UserDto(
    int Id,
    string FullName,
    string Email,
    int Role,
    string PhoneNumber,
    DateTime CreatedAt,
    bool IsActive)
{
    public static UserDto FromEntity(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        return new UserDto(
            user.Id,
            user.FullName,
            user.Email,
            user.Role switch
            {
                UserRole.Provider => 1,
                UserRole.Farmer => 2,
                UserRole.Admin => 3,
                _ => throw new ArgumentOutOfRangeException(nameof(user.Role))
            },
            user.PhoneNumber,
            user.CreatedAt,
            user.IsActive);
    }
}
