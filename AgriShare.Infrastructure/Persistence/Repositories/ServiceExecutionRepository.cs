using AgriShare.Application.Common.Interfaces;
using AgriShare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgriShare.Infrastructure.Persistence.Repositories;

public sealed class ServiceExecutionRepository : Repository<ServiceExecution>, IServiceExecutionRepository
{
    private readonly AppDbContext _context;

    public ServiceExecutionRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<ServiceExecution?> GetByServiceRequestIdAsync(
        int serviceRequestId,
        CancellationToken cancellationToken = default)
    {
        return _context.ServiceExecutions
            .FirstOrDefaultAsync(
                execution => execution.ServiceRequestId == serviceRequestId,
                cancellationToken);
    }
}
