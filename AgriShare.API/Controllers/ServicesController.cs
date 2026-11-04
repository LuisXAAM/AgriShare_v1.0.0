using AgriShare.Application.Services.Commands;
using AgriShare.Application.Services.DTOs;
using AgriShare.Application.Services.Queries;
using Microsoft.AspNetCore.Mvc;

namespace AgriShare.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ServicesController(
    CreateServiceCommandHandler createServiceHandler,
    GetServicesQueryHandler getServicesHandler,
    GetServiceByIdQueryHandler getServiceByIdHandler,
    UpdateServiceCommandHandler updateServiceHandler,
    DeactivateServiceCommandHandler deactivateServiceHandler,
    GetNearbyServicesQueryHandler getNearbyServicesHandler) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceDto>>> GetServices(
        CancellationToken cancellationToken)
    {
        var services = await getServicesHandler.Handle(
            new GetServicesQuery(),
            cancellationToken);

        return Ok(services);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServiceDto>> GetServiceById(
        int id,
        CancellationToken cancellationToken)
    {
        var service = await getServiceByIdHandler.Handle(
            new GetServiceByIdQuery(id),
            cancellationToken);

        return service is null ? NotFound() : Ok(service);
    }

    [HttpGet("nearby")]
    public async Task<ActionResult<IEnumerable<NearbyServiceDto>>> GetNearbyServices(
        [FromQuery] double latitude,
        [FromQuery] double longitude,
        [FromQuery] double radiusKm = 15.0,
        CancellationToken cancellationToken = default)
    {
        if (!AreValidCoordinates(latitude, longitude) ||
            !double.IsFinite(radiusKm) || radiusKm <= 0)
        {
            return BadRequest("Latitude/longitude must be valid coordinates and radiusKm must be positive.");
        }

        var services = await getNearbyServicesHandler.Handle(
            new GetNearbyServicesQuery(latitude, longitude, radiusKm),
            cancellationToken);

        return Ok(services);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceDto>> CreateService(
        [FromBody] CreateServiceDto serviceDto,
        CancellationToken cancellationToken)
    {
        if (!AreValidCoordinates(serviceDto.Latitude, serviceDto.Longitude))
        {
            return BadRequest("Latitude and longitude must be valid coordinates.");
        }

        var result = await createServiceHandler.Handle(
            new CreateServiceCommand(serviceDto),
            cancellationToken);

        return CreatedAtAction(nameof(GetServiceById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateService(
        int id,
        [FromBody] UpdateServiceDto serviceDto,
        CancellationToken cancellationToken)
    {
        if (serviceDto.Latitude.HasValue != serviceDto.Longitude.HasValue ||
            (serviceDto.Latitude is { } latitude &&
             serviceDto.Longitude is { } longitude &&
             !AreValidCoordinates(latitude, longitude)))
        {
            return BadRequest("Provide both latitude and longitude as valid coordinates, or omit both.");
        }

        var updated = await updateServiceHandler.Handle(
            new UpdateServiceCommand(id, serviceDto),
            cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteService(
        int id,
        CancellationToken cancellationToken)
    {
        var deactivated = await deactivateServiceHandler.Handle(
            new DeactivateServiceCommand(id),
            cancellationToken);

        return deactivated ? NoContent() : NotFound();
    }

    private static bool AreValidCoordinates(double latitude, double longitude)
    {
        return double.IsFinite(latitude) && latitude is >= -90 and <= 90 &&
               double.IsFinite(longitude) && longitude is >= -180 and <= 180;
    }
}
