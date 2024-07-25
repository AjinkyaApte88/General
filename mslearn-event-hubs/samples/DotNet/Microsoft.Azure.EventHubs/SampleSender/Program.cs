// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SampleSender
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    //using Microsoft.Azure.EventHubs;
    using Azure.Messaging.EventHubs;
    using Azure.Messaging.EventHubs.Producer;

    public class Program
    {
        private static EventHubProducerClient eventHubClient;
        private const string EventHubConnectionString = "Endpoint=sb://learneventhubsspace.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=YtXEEPtvVz//pzU9tW1Vl+cHFn8B4O1XOXyjgeuMpdw=";
        private const string EventHubName = "learneventhub";
        private static bool SetRandomPartitionKey = false;

        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
            UnicodeEncoding u = new UnicodeEncoding();
            byte[] b = u.GetBytes("");
        }
        private static async Task MainAsync(string[] args)
        {
            // Creates an EventHubsConnectionStringBuilder object from a the connection string, and sets the EntityPath.
            // Typically the connection string should have the Entity Path in it, but for the sake of this simple scenario
            // we are using the connection string from the namespace.
            
            eventHubClient = new EventHubProducerClient(EventHubConnectionString, EventHubName);

            await SendMessagesToEventHub(100);

            await eventHubClient.CloseAsync();

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        // Creates an Event Hub client and sends 100 messages to the event hub.
        private static async Task SendMessagesToEventHub(int numMessagesToSend)
        {
            var rnd = new Random();

            try
            {
                EventDataBatch eventBatch = await eventHubClient.CreateBatchAsync().Result;
                for (var i = 0; i < numMessagesToSend; i++)
                {
                    string message = $"Message {i}";

                    // Set random partition key?
                    //if (SetRandomPartitionKey)
                    //{
                    //    var pKey = Guid.NewGuid().ToString();
                    //    await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)), pKey);
                    //    Console.WriteLine($"Sent message: '{message}' Partition Key: '{pKey}'");
                    //}
                    //else
                    //{
                    //    await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
                    //    Console.WriteLine($"Sent message: '{message}'");
                    //}
                    if (!eventBatch.TryAdd(new EventData(new BinaryData(message))))
                        break;
                }

                await eventHubClient.SendAsync(eventBatch);
                Console.WriteLine($"Sent all messages");//: '{message}'");
                //await Task.Delay(10);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
            }

            Console.WriteLine($"{numMessagesToSend} messages sent.");
        }
    }
}
