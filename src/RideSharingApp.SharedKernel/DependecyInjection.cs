using Microsoft.Extensions.DependencyInjection;
using RideSharingApp.SharedKernel.DispacherEvent;

namespace RideSharingApp.SharedKernel;

public static class DependecyInjection
{
    public static IServiceCollection AddSharedKernel(this IServiceCollection services)
    {
        services.AddTransient<IEventPublisher, EventPublisher>();

        return services;
    }
}
