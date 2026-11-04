using System.ComponentModel.DataAnnotations;

namespace AgriShare.Application.Users.DTOs;

public sealed record UpdateUserDto(
    [property: Required, StringLength(200, MinimumLength = 1)] string FullName,
    [property: Required, StringLength(50)] string PhoneNumber);
