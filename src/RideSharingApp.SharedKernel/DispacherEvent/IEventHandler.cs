namespace RideSharingApp.Application.Common.DispacherEvent;

public interface IEventHandler<TEvent>
{
    Task HandleAsync(TEvent evt);
}
