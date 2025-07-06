using Microsoft.Extensions.DependencyInjection;

namespace RideSharingApp.SharedKernel
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddSharedKernel(this IServiceCollection services)
        {
            // Register shared kernel services here, if any
            // For example, logging, caching, etc.

            return services;
        }
    }
}
