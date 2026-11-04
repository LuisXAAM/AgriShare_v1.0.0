using AgriShare.Application.Common.Interfaces;
using AgriShare.Application.Services.DTOs;

namespace AgriShare.Application.Services.Queries;

public sealed record GetServicesQuery;

public sealed class GetServicesQueryHandler(IUnitOfWork unitOfWork)
{
    public async Task<IEnumerable<ServiceDto>> Handle(
        GetServicesQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        var services = await unitOfWork.Services.GetActiveWithDetailsAsync(cancellationToken);

        return services.Select(ServiceDto.FromEntity);
    }
}
