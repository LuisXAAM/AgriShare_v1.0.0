using AgriShare.Application.Common.Interfaces;
using AgriShare.Application.Services.DTOs;
using AgriShare.Domain.Entities;
using NetTopologySuite.Geometries;

namespace AgriShare.Application.Services.Commands;

public sealed record CreateServiceCommand(CreateServiceDto Service);

public sealed class CreateServiceCommandHandler(IUnitOfWork unitOfWork)
{
    public async Task<ServiceDto> Handle(
        CreateServiceCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(command.Service);

        var input = command.Service;
        if (!double.IsFinite(input.Latitude) || input.Latitude is < -90 or > 90)
        {
            throw new ArgumentOutOfRangeException(
                nameof(command), "Latitude must be between -90 and 90 degrees.");
        }

        if (!double.IsFinite(input.Longitude) || input.Longitude is < -180 or > 180)
        {
            throw new ArgumentOutOfRangeException(
                nameof(command), "Longitude must be between -180 and 180 degrees.");
        }

        var service = new Service
        {
            ProviderId = input.ProviderId,
            MachineryId = input.MachineryId,
            OperatorId = input.OperatorId,
            Title = input.Title,
            LaborType = input.LaborType,
            PricingUnit = input.PricingUnit,
            BasePrice = input.BasePrice,
            TransportFee = input.TransportFee,
            IsActive = input.IsActive,
            Location = new Point(input.Longitude, input.Latitude) { SRID = 4326 }
        };

        await unitOfWork.Services.AddAsync(service, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceDto.FromEntity(service);
    }
}
