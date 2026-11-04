using AgriShare.Application.Common.Interfaces;
using AgriShare.Application.Services.DTOs;
using NetTopologySuite.Geometries;

namespace AgriShare.Application.Services.Commands;

public sealed record UpdateServiceCommand(int Id, UpdateServiceDto Service);

public sealed class UpdateServiceCommandHandler(IUnitOfWork unitOfWork)
{
    public async Task<bool> Handle(
        UpdateServiceCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(command.Service);

        var input = command.Service;
        if (input.Latitude.HasValue != input.Longitude.HasValue)
        {
            throw new ArgumentException(
                "Latitude and longitude must either both be provided or both be omitted.",
                nameof(command));
        }

        if (input.Latitude is { } latitude &&
            (!double.IsFinite(latitude) || latitude is < -90 or > 90))
        {
            throw new ArgumentOutOfRangeException(
                nameof(command), "Latitude must be between -90 and 90 degrees.");
        }

        if (input.Longitude is { } longitude &&
            (!double.IsFinite(longitude) || longitude is < -180 or > 180))
        {
            throw new ArgumentOutOfRangeException(
                nameof(command), "Longitude must be between -180 and 180 degrees.");
        }

        var service = await unitOfWork.Services.GetByIdAsync(command.Id, cancellationToken);
        if (service is null)
        {
            return false;
        }

        service.Title = input.Title;
        service.LaborType = input.LaborType;
        service.PricingUnit = input.PricingUnit;
        service.BasePrice = input.BasePrice;
        service.TransportFee = input.TransportFee;
        service.IsActive = input.IsActive;

        if (input.Latitude is { } updatedLatitude &&
            input.Longitude is { } updatedLongitude)
        {
            service.Location = new Point(updatedLongitude, updatedLatitude) { SRID = 4326 };
        }

        await unitOfWork.Services.UpdateAsync(service, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
