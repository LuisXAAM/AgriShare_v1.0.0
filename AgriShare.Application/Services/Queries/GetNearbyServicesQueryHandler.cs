using AgriShare.Application.Common.Interfaces;
using AgriShare.Application.Services.DTOs;
using NetTopologySuite.Geometries;

namespace AgriShare.Application.Services.Queries;

public sealed record GetNearbyServicesQuery(double Latitude, double Longitude, double RadiusKm);

public sealed class GetNearbyServicesQueryHandler(IUnitOfWork unitOfWork)
{
    public async Task<IEnumerable<NearbyServiceDto>> Handle(
        GetNearbyServicesQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        if (!double.IsFinite(query.Latitude) || query.Latitude is < -90 or > 90)
        {
            throw new ArgumentOutOfRangeException(
                nameof(query), "Latitude must be between -90 and 90 degrees.");
        }

        if (!double.IsFinite(query.Longitude) || query.Longitude is < -180 or > 180)
        {
            throw new ArgumentOutOfRangeException(
                nameof(query), "Longitude must be between -180 and 180 degrees.");
        }

        if (!double.IsFinite(query.RadiusKm) || query.RadiusKm <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(query), "RadiusKm must be a positive finite number.");
        }

        var searchLocation = new Point(query.Longitude, query.Latitude) { SRID = 4326 };
        var services = await unitOfWork.Services.GetNearbyWithDetailsAsync(
            searchLocation,
            query.RadiusKm * 1000,
            cancellationToken);

        return services.Select(service => new NearbyServiceDto(
            service.Id,
            service.Title,
            service.BasePrice,
            service.Provider.BusinessName,
            CalculateDistanceKm(searchLocation, service.Location)));
    }

    private static double CalculateDistanceKm(Point origin, Point destination)
    {
        const double earthRadiusKm = 6371.0088;

        var latitudeDelta = DegreesToRadians(destination.Y - origin.Y);
        var longitudeDelta = DegreesToRadians(destination.X - origin.X);
        var originLatitude = DegreesToRadians(origin.Y);
        var destinationLatitude = DegreesToRadians(destination.Y);

        var haversine = Math.Pow(Math.Sin(latitudeDelta / 2), 2) +
                        Math.Cos(originLatitude) * Math.Cos(destinationLatitude) *
                        Math.Pow(Math.Sin(longitudeDelta / 2), 2);

        return earthRadiusKm * 2 * Math.Asin(Math.Sqrt(haversine));
    }

    private static double DegreesToRadians(double degrees) => degrees * Math.PI / 180;
}
