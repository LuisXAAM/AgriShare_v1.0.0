using AgriShare.Application.Common.Interfaces;
using AgriShare.Application.ServiceRequests.DTOs;
using AgriShare.Domain.Entities;
using AgriShare.Domain.Enums;
using NetTopologySuite.Geometries;

namespace AgriShare.Application.ServiceRequests.Commands;

public enum CreateServiceRequestFailure
{
    None,
    ServiceNotFoundOrInactive,
    FarmerNotFound,
    UserIsNotFarmer
}

public sealed record CreateServiceRequestResult(
    ServiceRequestDto? Request,
    CreateServiceRequestFailure Failure = CreateServiceRequestFailure.None);

public sealed record CreateServiceRequestCommand(CreateServiceRequestDto Request);

public sealed class CreateServiceRequestCommandHandler(IUnitOfWork unitOfWork)
{
    public async Task<CreateServiceRequestResult> Handle(
        CreateServiceRequestCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(command.Request);

        var input = command.Request;
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

        if (input.EstimatedSize <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(command), "Estimated size must be greater than zero.");
        }

        var service = await unitOfWork.Services.GetByIdAsync(input.ServiceId, cancellationToken);
        if (service is null || !service.IsActive)
        {
            return new CreateServiceRequestResult(
                null,
                CreateServiceRequestFailure.ServiceNotFoundOrInactive);
        }

        var farmer = await unitOfWork.Users.GetByIdAsync(input.FarmerId, cancellationToken);
        if (farmer is null)
        {
            return new CreateServiceRequestResult(null, CreateServiceRequestFailure.FarmerNotFound);
        }

        if (!farmer.IsActive || farmer.Role != UserRole.Farmer)
        {
            return new CreateServiceRequestResult(null, CreateServiceRequestFailure.UserIsNotFarmer);
        }

        var createdAt = DateTime.UtcNow;
        var serviceRequest = new ServiceRequest
        {
            FarmerId = input.FarmerId,
            ServiceId = input.ServiceId,
            FarmLocation = new Point(input.Longitude, input.Latitude) { SRID = 4326 },
            EstimatedSize = input.EstimatedSize,
            TotalCost = decimal.Round(
                service.BasePrice * input.EstimatedSize + service.TransportFee,
                2,
                MidpointRounding.AwayFromZero),
            Status = RequestStatus.Requested,
            ScheduledDate = NormalizeUtc(input.ScheduledDate),
            CreatedAt = createdAt
        };

        await unitOfWork.ServiceRequests.AddAsync(serviceRequest, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateServiceRequestResult(new ServiceRequestDto(
            serviceRequest.Id,
            serviceRequest.FarmerId,
            serviceRequest.ServiceId,
            serviceRequest.FarmLocation.Y,
            serviceRequest.FarmLocation.X,
            serviceRequest.EstimatedSize,
            serviceRequest.TotalCost,
            serviceRequest.Status,
            serviceRequest.ScheduledDate,
            serviceRequest.CreatedAt));
    }

    private static DateTime NormalizeUtc(DateTime dateTime)
    {
        return dateTime.Kind switch
        {
            DateTimeKind.Utc => dateTime,
            DateTimeKind.Local => dateTime.ToUniversalTime(),
            _ => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
        };
    }
}
