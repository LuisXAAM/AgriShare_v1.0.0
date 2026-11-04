using AgriShare.Application.Common.Interfaces;
using AgriShare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgriShare.Infrastructure.Persistence.Repositories;

public sealed class ReviewRepository(AppDbContext context)
    : Repository<Review>(context), IReviewRepository
{
    public Task<Review?> GetByExecutionIdAsync(
        int serviceExecutionId,
        CancellationToken cancellationToken = default)
    {
        return context.Reviews
            .AsNoTracking()
            .SingleOrDefaultAsync(
                review => review.ServiceExecutionId == serviceExecutionId,
                cancellationToken);
    }
}
