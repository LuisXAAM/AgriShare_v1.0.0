using AgriShare.Application.Common.Interfaces;
using AgriShare.Domain.Entities;

namespace AgriShare.Infrastructure.Persistence.Repositories;

public sealed class UnitOfWork(AppDbContext context) : IUnitOfWork, IDisposable
{
    private readonly AppDbContext _context = context;

    public IUserRepository Users { get; } = new UserRepository(context);
    public IRepository<Provider> Providers { get; } = new Repository<Provider>(context);
    public IRepository<Machinery> Machinery { get; } = new Repository<Machinery>(context);
    public IRepository<Operator> Operators { get; } = new Repository<Operator>(context);
    public IServiceRepository Services { get; } = new ServiceRepository(context);
    public IServiceRequestRepository ServiceRequests { get; } = new ServiceRequestRepository(context);
    public IServiceExecutionRepository ServiceExecutions { get; } = new ServiceExecutionRepository(context);
    public IReviewRepository Reviews { get; } = new ReviewRepository(context);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
