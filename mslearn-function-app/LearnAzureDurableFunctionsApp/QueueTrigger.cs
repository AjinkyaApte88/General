using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace LearnAzureDurableFunctionsApp
{
    public class QueueTrigger
    {
        [FunctionName("QueueTrigger")]
        public void Run(
            [QueueTrigger("newsqueue", Connection = "myConn")]string myQueueItem,
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            HttpClient client = new HttpClient();
            client.SendAsync(new HttpRequestMessage(HttpMethod.Post,
                "http://localhost:7271/api/OrchestrationFunction_HttpStart"));
        }
    }
}
