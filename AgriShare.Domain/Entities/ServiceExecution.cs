namespace AgriShare.Domain.Entities;

public class ServiceExecution
{
    public int Id { get; set; }
    public int ServiceRequestId { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? FieldNotes { get; set; }
    public string? OperatorSignatureUrl { get; set; }

    public ServiceRequest ServiceRequest { get; set; } = null!;
}
