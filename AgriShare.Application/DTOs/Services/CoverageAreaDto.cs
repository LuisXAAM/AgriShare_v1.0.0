namespace AgriShare.Application.DTOs.Services;

public sealed class CoverageAreaDto
{
    public IReadOnlyList<CoordinateDto> ExteriorRing { get; set; } = Array.Empty<CoordinateDto>();
    public IReadOnlyList<IReadOnlyList<CoordinateDto>> InteriorRings { get; set; }
        = Array.Empty<IReadOnlyList<CoordinateDto>>();
}
