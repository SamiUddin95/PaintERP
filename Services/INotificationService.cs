namespace PaintERP.Services;

public interface INotificationService
{
    void AddSuccessMessage(string message);
    void AddErrorMessage(string message);
    void AddWarningMessage(string message);
    void AddInfoMessage(string message);
    void AddValidationErrors(ValidationResult validationResult);
    List<NotificationMessage> GetMessages();
    void ClearMessages();
}

public class NotificationMessage
{
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public enum NotificationType
{
    Success,
    Error,
    Warning,
    Info
}
