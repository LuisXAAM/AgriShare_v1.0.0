using AgriShare.Domain.Entities;

namespace AgriShare.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IRepository<User> Users { get; }
    IRepository<Provider> Providers { get; }
    IRepository<Machinery> Machinery { get; }
    IRepository<Operator> Operators { get; }
    IRepository<Service> Services { get; }
    IRepository<ServiceRequest> ServiceRequests { get; }
    IRepository<ServiceExecution> ServiceExecutions { get; }
    IRepository<Review> Reviews { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
