using System.ComponentModel.DataAnnotations;

namespace AgriShare.Application.Users.DTOs;

public sealed record CreateUserDto(
    [property: Required, StringLength(200, MinimumLength = 1)] string FullName,
    [property: Required, EmailAddress, StringLength(320)] string Email,
    [property: Required, StringLength(500)] string PasswordHash,
    [property: Range(1, 2)] int Role,
    [property: Required, StringLength(50)] string PhoneNumber);
