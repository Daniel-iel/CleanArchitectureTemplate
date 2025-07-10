namespace RideSharingApp.SharedKernel.DispacherEvent;

public interface IEventHandler<TEvent>
{
    Task HandleAsync(TEvent evt);
}
