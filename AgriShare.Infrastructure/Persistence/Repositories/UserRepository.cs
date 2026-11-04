using AgriShare.Application.Common.Interfaces;
using AgriShare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgriShare.Infrastructure.Persistence.Repositories;

public sealed class UserRepository : Repository<User>, IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<bool> EmailExistsAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        var normalizedEmail = email.Trim().ToUpper();
        return _context.Users.AnyAsync(
            user => user.Email.ToUpper() == normalizedEmail,
            cancellationToken);
    }
}
