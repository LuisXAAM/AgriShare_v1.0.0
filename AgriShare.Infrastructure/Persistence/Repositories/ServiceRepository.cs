using AgriShare.Application.Common.Interfaces;
using AgriShare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace AgriShare.Infrastructure.Persistence.Repositories;

public sealed class ServiceRepository(AppDbContext context)
    : Repository<Service>(context), IServiceRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IReadOnlyList<Service>> GetActiveWithDetailsAsync(
        CancellationToken cancellationToken = default)
    {
        return await LoadWithDetails()
            .Where(service => service.IsActive)
            .ToListAsync(cancellationToken);
    }

    public Task<Service?> GetWithDetailsByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return LoadWithDetails()
            .FirstOrDefaultAsync(service => service.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Service>> GetNearbyWithDetailsAsync(
        Point location,
        double radiusMeters,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(location);

        return await _context.Services
            .FromSqlInterpolated($"""
                SELECT *
                FROM "Services"
                WHERE "IsActive" = TRUE
                  AND ST_DWithin(
                      "Location"::geography,
                      ST_SetSRID(ST_MakePoint({location.X}, {location.Y}), 4326)::geography,
                      {radiusMeters})
                """)
            .AsNoTracking()
            .Include(service => service.Provider)
            .Include(service => service.Machinery)
            .Include(service => service.Operator)
            .ToListAsync(cancellationToken);
    }

    private IQueryable<Service> LoadWithDetails()
    {
        return _context.Services
            .AsNoTracking()
            .Include(service => service.Provider)
            .Include(service => service.Machinery)
            .Include(service => service.Operator);
    }
}
