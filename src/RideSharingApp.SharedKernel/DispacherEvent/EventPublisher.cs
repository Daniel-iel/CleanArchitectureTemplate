using Microsoft.Extensions.DependencyInjection;
using RideSharingApp.SharedKernel.DomainEvents;
using System.Collections.Concurrent;

namespace RideSharingApp.SharedKernel.DispacherEvent;

internal sealed class EventPublisher(IServiceProvider serviceProvider) : IEventPublisher
{
    private static readonly ConcurrentDictionary<Type, Type> HandlerTypeDictionary = new();
    private static readonly ConcurrentDictionary<Type, Type> WrapperTypeDictionary = new();

    public async Task DispatchAsync(IDomainEvent @event, CancellationToken cancellationToken = default)
    {
        using IServiceScope scope = serviceProvider.CreateScope();

        Type domainEventType = @event.GetType();
        Type handlerType = HandlerTypeDictionary.GetOrAdd(
            domainEventType,
            et => typeof(IDomainEventHandler<>).MakeGenericType(et));

        IEnumerable<object?> handlers = scope.ServiceProvider.GetServices(handlerType);

        foreach (object? handler in handlers)
        {
            if (handler is null)
            {
                continue;
            }

            var handlerWrapper = HandlerWrapper.Create(handler, domainEventType);

            await handlerWrapper.HandleAsync(@event, cancellationToken);
        }
    }

    private abstract class HandlerWrapper
    {
        public abstract Task HandleAsync(IDomainEvent domainEvent, CancellationToken cancellationToken);

        public static HandlerWrapper Create(object handler, Type domainEventType)
        {
            Type wrapperType = WrapperTypeDictionary.GetOrAdd(
                domainEventType,
                et => typeof(HandlerWrapper<>).MakeGenericType(et));

            return (HandlerWrapper)Activator.CreateInstance(wrapperType, handler);
        }
    }

    private sealed class HandlerWrapper<T>(object handler) : HandlerWrapper where T : IDomainEvent
    {
        private readonly IDomainEventHandler<T> _handler = (IDomainEventHandler<T>)handler;

        public override Task HandleAsync(IDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            return _handler.HandleAsync((T)domainEvent, cancellationToken);
        }
    }
}