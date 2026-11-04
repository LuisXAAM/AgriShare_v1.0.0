using AgriShare.Domain.Entities;
using NetTopologySuite.Geometries;

namespace AgriShare.Application.Common.Interfaces;

public interface IServiceRepository : IRepository<Service>
{
    Task<IReadOnlyList<Service>> GetActiveWithDetailsAsync(
        CancellationToken cancellationToken = default);

    Task<Service?> GetWithDetailsByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Service>> GetNearbyWithDetailsAsync(
        Point location,
        double radiusMeters,
        CancellationToken cancellationToken = default);
}
