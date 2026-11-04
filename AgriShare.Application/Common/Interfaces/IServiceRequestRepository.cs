using AgriShare.Domain.Entities;

namespace AgriShare.Application.Common.Interfaces;

public interface IServiceRequestRepository : IRepository<ServiceRequest>
{
    Task<ServiceRequest?> GetWithDetailsByIdAsync(
        int id,
        CancellationToken cancellationToken = default);
}
