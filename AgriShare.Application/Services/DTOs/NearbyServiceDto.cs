namespace AgriShare.Application.Services.DTOs;

public sealed record NearbyServiceDto(
    int Id,
    string Title,
    decimal BasePrice,
    string ProviderName,
    double DistanceKm);
