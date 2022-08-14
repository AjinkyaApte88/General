// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;

namespace LearnEventGridFunctionApp
{
    public static class EventGridFunction1
    {
        //[EventGridTrigger]
        [FunctionName("EventGridFunction1")]
        public static void Run([EventGridTrigger]EventGridEvent gEvent, ILogger log)
        {
            string messageToWriteToFile = " *** Message from " + gEvent.Topic + " " + gEvent.Subject + " *** Type: " + gEvent.EventType + " *** Data: " + gEvent.Data.ToString();
            log.LogInformation(messageToWriteToFile);
        }
    }
}
