using AgriShare.Application.Common.Interfaces;
using AgriShare.Application.ServiceRequests.DTOs;

namespace AgriShare.Application.ServiceRequests.Queries;

public sealed record GetServiceRequestByIdQuery(int Id);

public sealed class GetServiceRequestByIdQueryHandler(IUnitOfWork unitOfWork)
{
    public async Task<ServiceRequestDetailDto?> Handle(
        GetServiceRequestByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        var request = await unitOfWork.ServiceRequests.GetWithDetailsByIdAsync(
            query.Id,
            cancellationToken);

        if (request is null)
        {
            return null;
        }

        return new ServiceRequestDetailDto(
            request.Id,
            request.FarmerId,
            request.Farmer.FullName,
            request.ServiceId,
            request.Service.Title,
            request.Service.Provider.BusinessName,
            request.FarmLocation.Y,
            request.FarmLocation.X,
            request.EstimatedSize,
            request.TotalCost,
            request.Status,
            request.ScheduledDate,
            request.CreatedAt);
    }
}
