using PaintERP.Services;

namespace PaintERP.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Core Services
        services.AddScoped<IValidationService, ValidationService>();
        services.AddScoped<IInventoryService, InventoryService>();
        services.AddScoped<IAccountingService, AccountingService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<UnitConversionService>();

        // HTTP Context Accessor for notifications
        services.AddHttpContextAccessor();

        return services;
    }
}
