namespace AgriShare.Domain.Enums;

public enum RequestStatus
{
    Requested,
    PendingConfirmation,
    Confirmed,
    InProgress,
    Completed,
    Cancelled,
    Disputed
}
