using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace privatemessagereceiver
{
    class Program
    {

        const string ServiceBusConnectionString = "Endpoint=sb://ajinkya-learn-service-bus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=EbY/Hp1Zg+nxOd+aNfwr657435pzbCYr04y8HQ087y0=";
        const string QueueName = "salesmessages";
        static ServiceBusProcessor processor;
        static void Main(string[] args)
        {

            ReceiveSalesMessageAsync().GetAwaiter().GetResult();

        }

        static async Task ReceiveSalesMessageAsync()
        {

            Console.WriteLine("======================================================");
            Console.WriteLine("Press ENTER key to exit after receiving all the messages.");
            Console.WriteLine("======================================================");


            var client = new ServiceBusClient(ServiceBusConnectionString);

            //var processorOptions = new ServiceBusProcessorOptions
            //{
            //    MaxConcurrentCalls = 1,
            //    AutoCompleteMessages = true
            //};

            //processor = client.CreateProcessor(QueueName, processorOptions);
            processor = client.CreateProcessor(QueueName);

            processor.ProcessMessageAsync += MessageHandler;
            processor.ProcessErrorAsync += ErrorHandler;

            await processor.StartProcessingAsync();

            Console.Read();

            await processor.CloseAsync();
        }

        // handle received messages
        static async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            Console.WriteLine($"Received: {body}");

            // complete the message. messages is deleted from the queue. 
            //await args.CompleteMessageAsync(args.Message);
        }

        // handle any errors when receiving messages
        static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
