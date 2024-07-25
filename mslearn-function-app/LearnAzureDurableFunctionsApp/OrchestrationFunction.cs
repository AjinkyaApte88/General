using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace LearnAzureDurableFunctionsApp
{
    public static class OrchestrationFunction
    {
        [FunctionName("OrchestrationFunction")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            // Replace "hello" with the name of your Durable Activity Function.
            OrchInput abc = context.GetInput<OrchInput>();
            Stopwatch stopwatch = Stopwatch.StartNew();
            outputs.Add(await context.CallActivityAsync<string>("OrchestrationFunction_Hello", "Tokyo"));
            
            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]

            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            outputs.Add(stopwatch.ElapsedMilliseconds.ToString());
            return outputs;
        }

        [FunctionName("OrchestrationFunction_Hello")]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");
            Thread.Sleep(1000);
            return $"Hello {name}!";
        }

        [FunctionName("OrchestrationFunction_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("OrchestrationFunction", new OrchInput());

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}