using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace performancemessagesender
{
    class Program
    {
        const string ServiceBusConnectionString = "Endpoint=sb://ajinkya-learn-service-bus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=EbY/Hp1Zg+nxOd+aNfwr657435pzbCYr04y8HQ087y0=";
        const string TopicName = "salesperformancemessages";

        static void Main(string[] args)
        {

            Console.WriteLine("Sending a message to the Sales Performance topic...");

            SendPerformanceMessageAsync().GetAwaiter().GetResult();

            Console.WriteLine("Message was sent successfully.");

        }

        static async Task SendPerformanceMessageAsync()
        {
            // By leveraging "await using", the DisposeAsync method will be called automatically when the client variable goes out of scope.
            // In more realistic scenarios, you would store off a class reference to the client (rather than to a local variable) so that it can be used throughout your program.
            await using var client = new ServiceBusClient(ServiceBusConnectionString);

            await using ServiceBusSender sender = client.CreateSender(TopicName);

            // Send messages.
            try
            {
                string messageBody = "Total sales for Brazil in August: $13m.";
                var message = new ServiceBusMessage(messageBody);
                Console.WriteLine($"Sending message: {messageBody}");
                await sender.SendMessageAsync(message);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }
    }
}
