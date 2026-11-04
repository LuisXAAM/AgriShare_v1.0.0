namespace AgriShare.Application.DTOs.Services;

public sealed class ServiceDto : CreateServiceDto
{
    public int Id { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public CoverageAreaDto? CoverageArea { get; set; }
}
