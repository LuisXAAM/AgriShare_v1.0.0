using AgriShare.Application.Users.Commands;
using AgriShare.Application.Users.DTOs;
using AgriShare.Application.Users.Queries;
using Microsoft.AspNetCore.Mvc;

namespace AgriShare.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UsersController(
    CreateUserCommandHandler createUserHandler,
    GetUsersQueryHandler getUsersHandler,
    GetUserByIdQueryHandler getUserByIdHandler,
    UpdateUserCommandHandler updateUserHandler,
    DeactivateUserCommandHandler deactivateUserHandler) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(
        [FromBody] CreateUserDto userDto,
        CancellationToken cancellationToken)
    {
        var result = await createUserHandler.Handle(
            new CreateUserCommand(userDto),
            cancellationToken);

        if (result.EmailAlreadyExists)
        {
            return BadRequest(new { Message = "An account with this email already exists." });
        }

        var user = result.User
            ?? throw new InvalidOperationException("User creation returned no result.");

        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserDto>>> GetUsers(
        CancellationToken cancellationToken)
    {
        var users = await getUsersHandler.Handle(new GetUsersQuery(), cancellationToken);
        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetUserById(
        int id,
        CancellationToken cancellationToken)
    {
        var user = await getUserByIdHandler.Handle(new GetUserByIdQuery(id), cancellationToken);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> UpdateUser(
        int id,
        [FromBody] UpdateUserDto userDto,
        CancellationToken cancellationToken)
    {
        var user = await updateUserHandler.Handle(
            new UpdateUserCommand(id, userDto),
            cancellationToken);

        return user is null ? NotFound() : Ok(user);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(
        int id,
        CancellationToken cancellationToken)
    {
        var deactivated = await deactivateUserHandler.Handle(
            new DeactivateUserCommand(id),
            cancellationToken);

        return deactivated ? NoContent() : NotFound();
    }
}
