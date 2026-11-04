using AgriShare.Domain.Enums;

namespace AgriShare.Domain.Entities;

public class Operator
{
    public int Id { get; set; }
    public int ProviderId { get; set; }
    public int? UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public OperatorStatus Status { get; set; }

    public Provider Provider { get; set; } = null!;
    public User? User { get; set; }
}
