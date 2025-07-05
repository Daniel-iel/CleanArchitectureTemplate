using Azure.Messaging.ServiceBus;
using RideSharingApp.Domain.Rides;

namespace RideSharingApp.Application.UseCases.Rides.Events
{
    public class RideRequestedEventHandler
    {
        private readonly ServiceBusClient serviceBusClient;

        public RideRequestedEventHandler(ServiceBusClient serviceBusClient)
        {
            this.serviceBusClient = serviceBusClient;
        }

        public Task HandleAsync(RideRequestedEvent rideRequestedEvent)
        {
            var sender = serviceBusClient.CreateSender("_topicName");
            var message = new ServiceBusMessage(System.Text.Json.JsonSerializer.Serialize(rideRequestedEvent));

            return sender.SendMessageAsync(message);
        }
    }
}
