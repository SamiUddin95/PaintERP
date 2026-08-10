using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace PaintERP.Services;

public class NotificationService : INotificationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string TempDataKey = "Notifications";

    public NotificationService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void AddSuccessMessage(string message)
    {
        AddMessage(message, NotificationType.Success);
    }

    public void AddErrorMessage(string message)
    {
        AddMessage(message, NotificationType.Error);
    }

    public void AddWarningMessage(string message)
    {
        AddMessage(message, NotificationType.Warning);
    }

    public void AddInfoMessage(string message)
    {
        AddMessage(message, NotificationType.Info);
    }

    public void AddValidationErrors(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
        {
            AddErrorMessage(error);
        }

        foreach (var warning in validationResult.Warnings)
        {
            AddWarningMessage(warning);
        }
    }

    public List<NotificationMessage> GetMessages()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
            return new List<NotificationMessage>();

        // Use HttpContext.Items for same-request notifications
        var serialized = httpContext.Items[TempDataKey] as string;
        
        if (string.IsNullOrEmpty(serialized))
            return new List<NotificationMessage>();

        return JsonConvert.DeserializeObject<List<NotificationMessage>>(serialized) ?? new List<NotificationMessage>();
    }

    public void ClearMessages()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            httpContext.Items.Remove(TempDataKey);
        }
    }

    private void AddMessage(string message, NotificationType type)
    {
        var messages = GetMessages();
        messages.Add(new NotificationMessage
        {
            Message = message,
            Type = type,
            Timestamp = DateTime.UtcNow
        });

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            httpContext.Items[TempDataKey] = JsonConvert.SerializeObject(messages);
        }
    }
}
