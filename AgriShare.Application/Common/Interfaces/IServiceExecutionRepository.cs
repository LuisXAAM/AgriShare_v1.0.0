using AgriShare.Domain.Entities;

namespace AgriShare.Application.Common.Interfaces;

public interface IServiceExecutionRepository : IRepository<ServiceExecution>
{
    Task<ServiceExecution?> GetByServiceRequestIdAsync(
        int serviceRequestId,
        CancellationToken cancellationToken = default);
}
