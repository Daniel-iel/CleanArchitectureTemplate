using Microsoft.Extensions.DependencyInjection;

namespace RideSharingApp.SharedKernel.DispacherEvent;

public class EventPublisher
{
    private readonly IServiceProvider _serviceProvider;

    public EventPublisher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task PublishAsync<TEvent>(TEvent evt)
    {
        var handler = _serviceProvider.GetService<IEventHandler<TEvent>>();
        if (handler == null)
        {
            throw new InvalidOperationException($"No handler registered for event type {typeof(TEvent).Name}");
        }

        return handler.HandleAsync(evt);
    }
}
