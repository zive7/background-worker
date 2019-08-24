namespace Processor.Api.Controllers
{
    using Hangfire;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    [RoutePrefix("process")]
    public class ProcessorController : ApiController
    {
        public ProcessorController()
        {

        }

        [HttpGet]
        [Route("test")]
        public void ProcessReportLog()
        {
            // Fire and forget tasks
            BackgroundJob.Enqueue(() => Console.WriteLine("Simple!"));
        }

        [HttpGet]
        [Route("test2")]
        public void ProcessScheduledTasks()
        {
            // Delayed tasks
            BackgroundJob.Schedule(() => Console.WriteLine("Reliable!"), TimeSpan.FromDays(7));
        }
    }
}