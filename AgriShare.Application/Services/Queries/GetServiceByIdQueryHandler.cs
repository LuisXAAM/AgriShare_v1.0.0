using AgriShare.Application.Common.Interfaces;
using AgriShare.Application.Services.DTOs;

namespace AgriShare.Application.Services.Queries;

public sealed record GetServiceByIdQuery(int Id);

public sealed class GetServiceByIdQueryHandler(IUnitOfWork unitOfWork)
{
    public async Task<ServiceDto?> Handle(
        GetServiceByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        var service = await unitOfWork.Services.GetWithDetailsByIdAsync(
            query.Id,
            cancellationToken);

        return service is null ? null : ServiceDto.FromEntity(service);
    }
}
