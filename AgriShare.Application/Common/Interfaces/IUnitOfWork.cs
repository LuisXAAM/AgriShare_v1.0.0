using AgriShare.Domain.Entities;

namespace AgriShare.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IRepository<Provider> Providers { get; }
    IRepository<Machinery> Machinery { get; }
    IRepository<Operator> Operators { get; }
    IServiceRepository Services { get; }
    IServiceRequestRepository ServiceRequests { get; }
    IServiceExecutionRepository ServiceExecutions { get; }
    IReviewRepository Reviews { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
