using System.ComponentModel.DataAnnotations;

namespace AgriShare.Application.ServiceRequests.DTOs;

public class CreateServiceRequestDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "El FarmerId debe ser válido.")]
    public int FarmerId { get; init; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "El ServiceId debe ser válido.")]
    public int ServiceId { get; init; }

    [Required]
    [Range(-90, 90, ErrorMessage = "La latitud debe estar entre -90 y 90.")]
    public double Latitude { get; init; }

    [Required]
    [Range(-180, 180, ErrorMessage = "La longitud debe estar entre -180 y 180.")]
    public double Longitude { get; init; }

    [Required]
    [Range(0.0001, 1000000.0, ErrorMessage = "El tamaño estimado debe ser mayor a cero.")]
    public decimal EstimatedSize { get; init; }

    [Required(ErrorMessage = "La fecha programada es obligatoria.")]
    public DateTime ScheduledDate { get; init; }
}