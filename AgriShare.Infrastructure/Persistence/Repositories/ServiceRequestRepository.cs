using AgriShare.Application.Common.Interfaces;
using AgriShare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgriShare.Infrastructure.Persistence.Repositories;

public sealed class ServiceRequestRepository : Repository<ServiceRequest>, IServiceRequestRepository
{
    private readonly AppDbContext _context;

    public ServiceRequestRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<ServiceRequest?> GetWithDetailsByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return _context.ServiceRequests
            .AsNoTracking()
            .Include(request => request.Service)
                .ThenInclude(service => service.Provider)
            .Include(request => request.Service)
                .ThenInclude(service => service.Machinery)
            .Include(request => request.Service)
                .ThenInclude(service => service.Operator)
            .Include(request => request.Farmer)
            .FirstOrDefaultAsync(request => request.Id == id, cancellationToken);
    }
}
