using AgriShare.Domain.Entities;

namespace AgriShare.Application.Common.Interfaces;

public interface IReviewRepository : IRepository<Review>
{
    Task<Review?> GetByExecutionIdAsync(
        int serviceExecutionId,
        CancellationToken cancellationToken = default);
}
