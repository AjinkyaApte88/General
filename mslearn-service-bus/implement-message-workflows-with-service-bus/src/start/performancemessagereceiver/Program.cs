using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace performancemessagereceiver
{
    class Program
    {
        const string ServiceBusConnectionString = "Endpoint=sb://ajinkya-learn-service-bus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=EbY/Hp1Zg+nxOd+aNfwr657435pzbCYr04y8HQ087y0=";
        const string TopicName = "salesperformancemessages";
        const string SubscriptionName = "Americas";

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            var client = new ServiceBusClient(ServiceBusConnectionString);

            Console.WriteLine("======================================================");
            Console.WriteLine("Press ENTER key to exit after receiving all the messages.");
            Console.WriteLine("======================================================");

            //var processorOptions = new ServiceBusProcessorOptions
            //{
            //    MaxConcurrentCalls = 1,
            //    AutoCompleteMessages = false
            //};

            //ServiceBusProcessor processor = client.CreateProcessor(TopicName, SubscriptionName, processorOptions);
            ServiceBusProcessor processor = client.CreateProcessor(TopicName, SubscriptionName);

            processor.ProcessMessageAsync += MessageHandler;
            processor.ProcessErrorAsync += ErrorHandler;

            await processor.StartProcessingAsync();

            Console.Read();

            await processor.DisposeAsync();
            await client.DisposeAsync();
        }

        static Task MessageHandler(ProcessMessageEventArgs args)
        {
            Console.WriteLine($"Received message: SequenceNumber:{args.Message.SequenceNumber} Body:{args.Message.Body}");
            //await args.CompleteMessageAsync(args.Message);
            return Task.CompletedTask;
        }

        static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"Message handler encountered an exception {args.Exception}.");
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {args.FullyQualifiedNamespace}");
            Console.WriteLine($"- Entity Path: {args.EntityPath}");
            Console.WriteLine($"- Executing Action: {args.ErrorSource}");
            return Task.CompletedTask;
        }
    }
}
