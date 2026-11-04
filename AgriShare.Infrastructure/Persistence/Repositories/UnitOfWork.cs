using AgriShare.Application.Common.Interfaces;
using AgriShare.Domain.Entities;

namespace AgriShare.Infrastructure.Persistence.Repositories;

public sealed class UnitOfWork(AppDbContext context) : IUnitOfWork, IDisposable
{
    private readonly AppDbContext _context = context;

    public IRepository<User> Users { get; } = new Repository<User>(context);
    public IRepository<Provider> Providers { get; } = new Repository<Provider>(context);
    public IRepository<Machinery> Machinery { get; } = new Repository<Machinery>(context);
    public IRepository<Operator> Operators { get; } = new Repository<Operator>(context);
    public IRepository<Service> Services { get; } = new Repository<Service>(context);
    public IRepository<ServiceRequest> ServiceRequests { get; } = new Repository<ServiceRequest>(context);
    public IRepository<ServiceExecution> ServiceExecutions { get; } = new Repository<ServiceExecution>(context);
    public IRepository<Review> Reviews { get; } = new Repository<Review>(context);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
