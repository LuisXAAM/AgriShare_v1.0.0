using AgriShare.Domain.Entities;

namespace AgriShare.Application.Common.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<bool> EmailExistsAsync(
        string email,
        CancellationToken cancellationToken = default);
}
