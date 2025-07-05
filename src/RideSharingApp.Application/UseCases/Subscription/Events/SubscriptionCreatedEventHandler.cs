using Azure.Messaging.ServiceBus;
using RideSharingApp.Domain.Subscriptions;

namespace RideSharingApp.Application.UseCases.Subscription.Events
{
    public class SubscriptionCreatedEventHandler
    {
        private readonly ServiceBusClient serviceBusClient;

        public SubscriptionCreatedEventHandler(ServiceBusClient serviceBusClient)
        {
            this.serviceBusClient = serviceBusClient;
        }

        public Task Handle(SubscriptionCreatedEvent subscriptionCreatedEvent)
        {
            // Handle the subscription created event, e.g., log it, notify users, etc.
            var sender = serviceBusClient.CreateSender("_topicName");
            var message = new ServiceBusMessage(System.Text.Json.JsonSerializer.Serialize(subscriptionCreatedEvent));

            return sender.SendMessageAsync(message);
        }
    }
}
