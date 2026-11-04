using AgriShare.Application.ServiceRequests.Commands;
using AgriShare.Application.ServiceRequests.DTOs;
using AgriShare.Application.ServiceRequests.Queries;
using Microsoft.AspNetCore.Mvc;

namespace AgriShare.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ServiceRequestsController(
    CreateServiceRequestCommandHandler createServiceRequestHandler,
    GetServiceRequestByIdQueryHandler getServiceRequestByIdHandler) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ServiceRequestDto>> CreateServiceRequest(
        [FromBody] CreateServiceRequestDto request,
        CancellationToken cancellationToken)
    {
        var createdRequest = await createServiceRequestHandler.Handle(
            new CreateServiceRequestCommand(request),
            cancellationToken);

        if (createdRequest.Failure is CreateServiceRequestFailure.ServiceNotFoundOrInactive or
            CreateServiceRequestFailure.FarmerNotFound)
        {
            return NotFound("The farmer or active service was not found.");
        }

        if (createdRequest.Failure == CreateServiceRequestFailure.UserIsNotFarmer)
        {
            return BadRequest("The specified user is not an active farmer.");
        }

        var requestDto = createdRequest.Request
            ?? throw new InvalidOperationException("Service request creation returned no result.");

        return CreatedAtAction(
            nameof(GetServiceRequestById),
            new { id = requestDto.Id },
            requestDto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServiceRequestDetailDto>> GetServiceRequestById(
        int id,
        CancellationToken cancellationToken)
    {
        var request = await getServiceRequestByIdHandler.Handle(
            new GetServiceRequestByIdQuery(id),
            cancellationToken);

        return request is null ? NotFound() : Ok(request);
    }
}
